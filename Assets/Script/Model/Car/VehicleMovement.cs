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
        private float currentSteerAngle,
            currentBrakeForce;
        private bool isBraking;
        private Rigidbody _rb;

        [Space]
        [Header("Settings")]
        [SerializeField]
        private float motorForce,
            brakeForce,
            maxSteerAngle;

        [Space]
        [Header("Wheel colliders")]
        [SerializeField]
        private WheelCollider frontLeftWheelCollider,
            frontRightWheelCollider;

        [SerializeField]
        private WheelCollider rearLeftWheelCollider,
            rearRightWheelCollider;

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

        private Driver driver;

        private void Awake() => _rb = GetComponent<Rigidbody>();

        private void Start()
        {
            GameManager.Instance.RegisterVehicle(this);
            driver = PlayerManager.Instance.Driver;
            driver.OnAccelerate += (object sender, float input) => throttle = input;
            driver.OnSteer += (object sender, float input) => steer = input;
        }

        private void FixedUpdate()
        {
            HandleMotor(throttle);
            HandleSteering(steer);
            UpdateWheels();
        }

        private void HandleMotor(float verticalInput)
        {
            frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
            frontRightWheelCollider.motorTorque = verticalInput * motorForce;
            // currentbreakForce = isBreaking ? breakForce : 0f;
            // ApplyBreaking();
        }

        // private void ApplyBreaking() {
        //     frontRightWheelCollider.brakeTorque = currentbreakForce;
        //     frontLeftWheelCollider.brakeTorque = currentbreakForce;
        //     rearLeftWheelCollider.brakeTorque = currentbreakForce;
        //     rearRightWheelCollider.brakeTorque = currentbreakForce;
        // }

        private void HandleSteering(float horizontalInput)
        {
            currentSteerAngle = maxSteerAngle * horizontalInput;
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
    }
}
