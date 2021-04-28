using UnityEngine;

namespace RG.Match3 {
  public class GameTile {
    public GameObject tile;
    public int type;
    private bool isAnimating;
    private float animTime;
    private float stepsCount;
    private float stepsStartFrom;
    public GameTile(GameObject tile, int type) {
      this.tile = tile;
      this.type = type;
    }
      
    public void DropTile() {
      if (isAnimating) {
        stepsCount += 1f;
      }
      else {
        stepsStartFrom = tile.transform.position.y;
        stepsCount = 1f;
        animTime = Time.deltaTime;
      }
      isAnimating = true;
    }

    public bool Animate() {
      isAnimating = animTime > 0.0f && animTime <= 1.0f;
      if (isAnimating) {
        tile.transform.position = new Vector3(tile.transform.position.x, -(Utils.Easings.BounceOut(animTime) * stepsCount) + stepsStartFrom, 0f);
        animTime += Time.deltaTime;
      }
      return isAnimating;
    }
  }
}