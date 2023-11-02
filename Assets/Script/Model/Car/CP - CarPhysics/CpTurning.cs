using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver
{
    public class CpTurning : MonoBehaviour
    {
        public float baseTurningForce;
        public float speedFactorOffset = 0.25f;

        [Space]
        public float currentTurningForce;
        public Vector3 currentAngularVelocity;
        public float airControlFactor;

        private VehicleMovement cpMain;

        private void Awake()
        {
            cpMain = transform.parent.GetComponent<VehicleMovement>();
        }

        // Start is called before the first frame update
        void Start()
        {
            currentTurningForce = baseTurningForce;
        }

        private void Update()
        {
            currentAngularVelocity = cpMain.rb.angularVelocity;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            ApplyTurningForce(cpMain.input, cpMain.speedData, cpMain.rb, cpMain.wheelData.grounded);
            AirTurning(cpMain.input, cpMain.rb, cpMain.wheelData.grounded);
        }

        private void ApplyTurningForce(
            PlayerInputs input,
            VehicleSpeed speedData,
            Rigidbody rigidbody,
            bool grounded
        )
        {
            if (input.steeringInput == 0)
                return;

            if (!grounded)
                return;

            //Adjusts turning with speed
            float speedFactor = Mathf.Clamp01(speedData.SpeedPercent + speedFactorOffset);
            float rotationTorque =
                input.steeringInput * baseTurningForce * speedFactor * Time.fixedDeltaTime;

            //Apply the torque to the ship's Y axis
            rigidbody.AddRelativeTorque(0f, rotationTorque, 0f, ForceMode.VelocityChange);
        }

        public void AirTurning(PlayerInputs input, Rigidbody rigidbody, bool grounded)
        {
            if (Math.Abs(input.steeringInput) < 0.01f)
                return;

            if (grounded)
                return;

            float rotationTorque =
                input.steeringInput * baseTurningForce * Time.fixedDeltaTime * airControlFactor;
            rigidbody.AddRelativeTorque(
                0f,
                rotationTorque,
                0f,
                ForceMode.VelocityChange
            );
        }
    }
}
