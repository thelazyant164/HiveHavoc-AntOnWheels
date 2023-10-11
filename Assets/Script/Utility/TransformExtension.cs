using System.Collections;
using UnityEngine;

public static class ITransformExtension
{
    public static IEnumerator LerpTo(
        this Transform transform,
        Vector3 targetPosition,
        float duration
    )
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                elapsedTime / duration
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
}
