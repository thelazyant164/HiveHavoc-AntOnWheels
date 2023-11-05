using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformExtension
{
    public static void MatchDimensionTo(this RectTransform rt, RectTransform other)
    {
        Vector2 myPrevPivot = rt.pivot;
        rt.pivot = other.pivot;
        rt.localScale = other.localScale;

        rt.MatchDimensionTo(other.rect);
        //rectTransf.ForceUpdateRectTransforms(); - needed before we adjust pivot a second time?
        rt.pivot = myPrevPivot;
    }

    public static void MatchDimensionTo(this RectTransform rt, Rect other)
    {
        rt.position = other.position;
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, other.width);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, other.height);
    }

    public static IEnumerator LerpTo(
        this RectTransform rt,
        Vector2 targetAnchoredPosition,
        float duration
    )
    {
        Vector2 startPosition = rt.anchoredPosition;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            rt.anchoredPosition = Vector2.Lerp(
                startPosition,
                targetAnchoredPosition,
                elapsedTime / duration
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rt.anchoredPosition = targetAnchoredPosition;
    }
}
