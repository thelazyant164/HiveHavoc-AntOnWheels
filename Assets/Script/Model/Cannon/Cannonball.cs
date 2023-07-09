using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Com.Unnamed.RacingGame.Shooter
{
    public sealed class Cannonball : MonoBehaviour
    {
        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        internal void Launch(Vector3 force) => rb.AddForce(force, ForceMode.Impulse);

        //private void OnCollisionEnter(Collision collision)
        //{
        //    GameObject.Destroy(gameObject);
        //}
    }
}
