using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cpFollowPhysicsBody : MonoBehaviour
{
    [SerializeField]
    private Transform physicsBodyTransform;

    // Start is called before the first frame update
    void Start()
    {
        if (physicsBodyTransform == null)
        {
            Debug.LogError("No physics body attach!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = physicsBodyTransform.position;
        transform.rotation = physicsBodyTransform.rotation;
    }
}
