using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public static IEnumerator ScaleTo(
        this GameObject gameObject,
        Vector3 targetScale,
        float duration
    )
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

    private static IEnumerator ExecAfterTimeOut(float timeOut, Action callback)
    {
        yield return new WaitForSeconds(timeOut);
        callback();
    }

    public static void SetTimeOut(this GameObject gameObject, float timeOut, Action callback)
    {
        gameObject
            .GetComponent<MonoBehaviour>()
            .StartCoroutine(ExecAfterTimeOut(timeOut, callback));
    }

    public static bool InLayerMask(this GameObject gameObject, LayerMask layerMask) =>
        (layerMask.value & (1 << gameObject.layer)) > 0;

    /// <summary>
    /// Utility method to try and find an immediate component of type in the same game object, or in its first parent.
    /// </summary>
    /// <typeparam name="T">Type of component to find.</typeparam>
    /// <param name="gameObject">Game object to perform search on.</param>
    /// <param name="component">The found component.</param>
    /// <returns>True if component of matching type is found.</returns>
    public static bool TryFindImmediateComponent<T>(this GameObject gameObject, out T component)
    {
        component =
            gameObject.GetComponent<T>()
            ?? gameObject.transform.parent.gameObject.GetComponent<T>();
        return component != null;
    }
}
