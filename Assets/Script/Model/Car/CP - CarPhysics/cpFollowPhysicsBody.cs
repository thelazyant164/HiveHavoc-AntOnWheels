using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class cpFollowPhysicsBody : MonoBehaviour
{
    [SerializeField]
    private Transform physicsBodyTransform;

    private void Awake()
    {
        Assert.IsNotNull(physicsBodyTransform);
    }

    private void Update()
    {
        if (Time.timeScale == 0)
            return;
        transform.SetPositionAndRotation(
            physicsBodyTransform.position,
            physicsBodyTransform.rotation
        );
    }
}
