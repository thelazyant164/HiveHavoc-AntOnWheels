using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver
{
    public sealed class VehicleMovement : MonoBehaviour
    {
        [Header("Current input")]
        [SerializeField]
        private float throttle;

        [SerializeField]
        private float steer;

        [SerializeField]
        private bool brake;

        [SerializeField]
        private float thruster;

        [SerializeField]
        private float actualThruster;

        [Space]
        [Header("Settings")]
        [SerializeField]
        private float motorForce;

        [SerializeField]
        private float brakeForce;

        [SerializeField]
        private float steerAngle;

        [SerializeField]
        private float thrusterForce;

        [SerializeField]
        private float thrusterDeteriorateRate;

        [Space]
        [Header("Wheel colliders")]
        [SerializeField]
        private WheelCollider frontLeftWheelCollider;

        [SerializeField]
        private WheelCollider frontRightWheelCollider;

        [SerializeField]
        private WheelCollider rearLeftWheelCollider;

        [SerializeField]
        private WheelCollider rearRightWheelCollider;

        [Space]
        [Header("Wheels")]
        [SerializeField]
        private Transform frontLeftWheelTransform;

        [SerializeField]
        private Transform frontRightWheelTransform;

        [SerializeField]
        private Transform rearLeftWheelTransform;

        [SerializeField]
        private Transform rearRightWheelTransform;

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            StartCoroutine(ThrusterDeteriorate());
        }

        private void Start()
        {
            GameManager.Instance.RegisterVehicle(this);
            Driver driver = PlayerManager.Instance.Driver;
            driver.OnAccelerate += (object sender, float input) => throttle = brake ? 0f : input;
            driver.OnSteer += (object sender, float input) => steer = input;
            driver.OnBrake += (object sender, bool brake) => this.brake = brake;

            Shooter.Shooter shooter = PlayerManager.Instance.Shooter;
            shooter.OnThruster += (object sender, float thruster) => this.thruster = thruster;
        }

        private void FixedUpdate()
        {
            HandleThruster();
            HandleMotor(throttle);
            HandleSteering(steer);
            UpdateWheels();
        }

        private void HandleThruster()
        {
            rb.AddForce(Vector3.up * thruster * actualThruster, ForceMode.Force);
        }

        private void HandleMotor(float verticalInput)
        {
            frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
            frontRightWheelCollider.motorTorque = verticalInput * motorForce;
            ApplyBraking(brake ? brakeForce : 0f);
        }

        private void ApplyBraking(float brakeForce)
        {
            frontRightWheelCollider.brakeTorque = brakeForce;
            frontLeftWheelCollider.brakeTorque = brakeForce;
            rearLeftWheelCollider.brakeTorque = brakeForce;
            rearRightWheelCollider.brakeTorque = brakeForce;
        }

        private void HandleSteering(float horizontalInput)
        {
            float currentSteerAngle = steerAngle * horizontalInput;
            frontLeftWheelCollider.steerAngle = currentSteerAngle;
            frontRightWheelCollider.steerAngle = currentSteerAngle;
        }

        private void UpdateWheels()
        {
            UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
            UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
            UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
            UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        }

        private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
        {
            wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
            wheelTransform.rotation = rot;
            wheelTransform.position = pos;
        }

        private IEnumerator ThrusterDeteriorate()
        {
            float thrusterBurnDuration = 0;
            while (true)
            {
                if (thruster == 0 || actualThruster <= 0)
                {
                    actualThruster = thrusterForce;
                    thrusterBurnDuration = 0;
                }
                thrusterBurnDuration += Time.deltaTime;
                actualThruster -= thrusterBurnDuration * thrusterDeteriorateRate;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
