using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class IGameObjectExtension
{
    private static List<GameObject> FindChildrenWithTag(this Transform parent, string tag)
    {
        List<GameObject> taggedGameObjects = new List<GameObject>();

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == tag)
            {
                taggedGameObjects.Add(child.gameObject);
            }
            if (child.childCount > 0)
            {
                taggedGameObjects.AddRange(FindChildrenWithTag(child, tag));
            }
        }
        return taggedGameObjects;
    }

    public static GameObject FindChildWithTag(this GameObject parent, string tag)
    {
        return parent.transform.FindChildrenWithTag(tag).FirstOrDefault();
    }

    public static IEnumerator ScaleTo(this GameObject gameObject, Vector3 targetScale, float duration)
    {
        Vector3 initialScale = gameObject.transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            gameObject.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.localScale = targetScale;
    }
}
