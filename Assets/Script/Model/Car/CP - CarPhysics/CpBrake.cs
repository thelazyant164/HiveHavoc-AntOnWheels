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
                cpMain.rb.AddForce(-brakeFactor * cpMain.rb.velocity);
            }
        }
    }
}