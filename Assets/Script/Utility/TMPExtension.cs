using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class ITMPExtension
{
    public static IEnumerator Fade(this TMP_Text textMeshPro, float duration)
    {
        float elapsedTime = 0f;
        Color startColor = textMeshPro.color;
        Color targetColor = startColor;
        targetColor.a = 1f - startColor.a;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            textMeshPro.color = Color.Lerp(startColor, targetColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textMeshPro.color = targetColor;
    }
}
