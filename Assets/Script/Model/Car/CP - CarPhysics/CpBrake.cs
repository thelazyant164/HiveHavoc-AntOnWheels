using System;
using System.Collections;
using UnityEngine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver
{
    public class CpBrake : MonoBehaviour
    {

        public float brakeFactor;

        private VehicleMovement cpMain;

        private void Awake()
        {
            cpMain = transform.parent.GetComponent<VehicleMovement>();
        }

        private void FixedUpdate()
        {
            if (cpMain.input.brake) 
            {
                ApplyBrake(cpMain.rb, cpMain.wheelData.grounded);
            }
        }

        private void ApplyBrake(Rigidbody rb,bool grounded)
        {
            if (!grounded)
                return;
            rb.AddForce(-brakeFactor * rb.velocity);
        }
    }
}