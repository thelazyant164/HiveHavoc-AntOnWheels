using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectExtension
{
    internal static Rect Lerp(Rect start, Rect target, float ratio)
    {
        float x = Mathf.Lerp(start.x, target.x, ratio);
        float y = Mathf.Lerp(start.y, target.y, ratio);
        float width = Mathf.Lerp(start.width, target.width, ratio);
        float height = Mathf.Lerp(start.height, target.height, ratio);
        return new Rect(x, y, width, height);
    }
}
