using System.Collections.Generic;
using UnityEngine;

namespace RG.Match3 {
  public class GameWorld : MonoBehaviour {

    [SerializeField]
    private List<GameObject> tilesPrefabs = new List<GameObject>();

    [SerializeField]
    private int gridRows = 5;

    [SerializeField]
    private int gridCols = 5;

    private GameTile[,] tilesGrid;

    //for collecting similar tiles to remove
    private int[] removingTiles;

    void Start() {
      tilesGrid = new GameTile[gridCols, gridRows];
      removingTiles = new int[gridCols];
      // prepare the grid of randomlly selected tiles from the list
      for (int row = 0; row < gridRows; row++) {
        for (int col = 0; col < gridCols; col++) {
          int type = Random.Range(0, tilesPrefabs.Count);
          GameObject tile = (GameObject)Instantiate(tilesPrefabs[type], new Vector3(col + 0.5f, -(row + 0.5f), 0), Quaternion.identity);
          tile.transform.parent = transform;
          GameTile gameTile = new GameTile(tile, type);
          tilesGrid[col, row] = gameTile;
        }
      }
      transform.position = new Vector3(-gridCols * 0.5f, gridRows * 0.5f, 0.0f);
    }



    private void DestroyRemovingTiles(int row, int count) {
      GameTile gameTile;
      int col;
      while (--count > -1) {
        col = removingTiles[count];
        gameTile = tilesGrid[col, row];
        tilesGrid[col, row] = null;
        Destroy(gameTile.tile);
      }
    }

    void RemoveThreeOrMoreTiles() {
      int previousTileType, removingTilesCount;
      GameTile gameTile;

      for (int row = gridRows - 1; row > -1; row--) {
        removingTilesCount = 0;
        previousTileType = -1;
        for (int col = 0; col < gridCols; col++) {
          gameTile = tilesGrid[col, row];
          if (gameTile != null) {
            // if the type of current tile does not match previous tile ,then check if we collected atleast 3 tiles to destroy
            if (gameTile.type != previousTileType) {
              if (removingTilesCount > 2) {
                DestroyRemovingTiles(row, removingTilesCount);                
              }
              removingTilesCount = 0;
            }
            previousTileType = gameTile.type;
            removingTiles[removingTilesCount++] = col;
          }
          else {
            if (removingTilesCount > 2) {
              DestroyRemovingTiles(row, removingTilesCount);              
            }
            removingTilesCount = 0;
            previousTileType = -1;
          }

        }
        if (removingTilesCount > 2) {
          DestroyRemovingTiles(row, removingTilesCount);          
        }
      }
    }

    // analyze the grid and move or remove tiles if needed
    void ValidateTilesGrid() {

      int row, col;
      GameTile gameTile;

      bool tilesAreAnimating = false;

      //perform the dropping animation
      for (row = 0; row < gridRows; row++)
      {
        for (col = 0; col < gridCols; col++)
        {
          gameTile = tilesGrid[col, row];
          if (gameTile != null)
          {
            if (gameTile.Animate())
            {
              tilesAreAnimating = true;
            }
          }
        }
      }

      // if tiles are not animating check for pair of tiles to remove
      if (!tilesAreAnimating) RemoveThreeOrMoreTiles();

      // Check the tiles needs to move in next row
      for (row = 0; row < gridRows - 1; row++) {
        for (col = 0; col < gridCols; col++) {
          gameTile = tilesGrid[col, row];
          if (gameTile != null) {
            if (tilesGrid[col, row + 1] == null) {
              tilesGrid[col, row + 1] = gameTile;
              tilesGrid[col, row] = null;
              gameTile.DropTile();

            }
          }
        }
      }

     

    }

    
    void Update() {
      if (Input.GetMouseButtonDown(0)) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
          int id = hit.collider.gameObject.GetInstanceID();
          for (int row = 0; row < gridRows; row++) {
            for (int col = 0; col < gridCols; col++) {
              if (tilesGrid[col, row] != null) {
                if (tilesGrid[col, row].tile.GetInstanceID() == id) {
                  tilesGrid[col, row] = null;
                  Destroy(hit.collider.gameObject);
                }
              }
            }
          }
        }
      }

      ValidateTilesGrid();
    }
  }

}