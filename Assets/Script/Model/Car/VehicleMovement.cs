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
        [Header("Rotation Clamping")]
        [SerializeField]
        private float maxPitch = 45f; // Max pitch angle in degrees
        [SerializeField]
        private float maxRoll = 30f;  // Max roll angle in degrees

        [Header("Dampening")]
        [SerializeField] 
        private float dampeningThreshold = 10f; // Start dampening when 10 degrees away from max
        [SerializeField]
        private float dampeningAmount = 3f;


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
            DampenRotation();
            ClampRotation();
            HandleThruster();
            HandleMotor(throttle);
            HandleSteering(steer);
            UpdateWheels();
        }
        

        internal void Respawn(Transform transform)
        {
            rb.transform.position = transform.position;
            rb.transform.rotation = transform.rotation;
            rb.velocity = Vector3.zero;
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

        private void ClampRotation()
        {
            Vector3 eulerRotation = transform.eulerAngles;
            
            // Ensure angles are between 0 and 360
            if (eulerRotation.x > 180) eulerRotation.x -= 360;
            if (eulerRotation.z > 180) eulerRotation.z -= 360;

            eulerRotation.x = Mathf.Clamp(eulerRotation.x, -maxPitch, maxPitch);
            eulerRotation.z = Mathf.Clamp(eulerRotation.z, -maxRoll, maxRoll);

            transform.eulerAngles = eulerRotation;

            // Debug.Log("Roll Amount: " + eulerRotation.z);
        }
        private void DampenRotation()
        {
            Vector3 eulerRotation = transform.eulerAngles;
            
            float pitchDampening = Mathf.InverseLerp(maxPitch - dampeningThreshold, maxPitch, eulerRotation.x);
            float rollDampening = Mathf.InverseLerp(maxRoll - dampeningThreshold, maxRoll, eulerRotation.z);
            
            Vector3 angularVelocity = rb.angularVelocity;
            angularVelocity.x *= pitchDampening / dampeningAmount; 
            angularVelocity.z *= rollDampening / dampeningAmount;  
            rb.angularVelocity = angularVelocity;
            Debug.Log("Roll Dampening: " + rollDampening + ", AngularVelocity: " + angularVelocity.z);
        }

    }
}
