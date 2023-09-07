using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColliderExtension
{
    public static float GetBoundVolume(this Collider collider) 
        => collider.bounds.size.x 
        * collider.bounds.size.y 
        * collider.bounds.size.z;
}
