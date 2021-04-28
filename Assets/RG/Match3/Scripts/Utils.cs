using UnityEngine;

namespace RG.Match3.Utils {
  //Some easings for nice tile dropping animations
  public class Easings {
    public static float BounceOut(float t) {
      if (t < (1f / 2.75f)) { 
        return 7.5625f * t * t; 
      }
      else if (t < (2 / 2.75f)) { 
        return 7.5625f * (t -= (1.5f / 2.75f)) * t + 0.75f; 
      }
      else if (t < (2.5f / 2.75f)) { 
        return 7.5625f * (t -= (2.25f / 2.75f)) * t + 0.9375f; 
      }
      else { 
        return 7.5625f * (t -= (2.625f / 2.75f)) * t + 0.984375f; 
      }
    }

    public static float BackOut(float t) {
      return --t * t * ((1.70158f + 1f) * t + 1.70158f) + 1f;
    }

    public static float CubicIn(float t)
    {
      return t * t * t; 
    }

    public static float ExponentialOut(float t) {
      return 1f - Mathf.Pow(2f, -10f * t);
    }
  }
}