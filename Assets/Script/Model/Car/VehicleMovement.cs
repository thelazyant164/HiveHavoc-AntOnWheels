using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver
{
    public enum WheelLayout
    {
        FrontLeft,
        FrontRight,
        RearLeft,
        RearRight
    }

    [Serializable]
    public sealed class WheelSet : SerializableDictionary<WheelLayout, Wheel> { }

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
        private float flightSteerPropulsion;

        [SerializeField]
        private float thrusterForce;
        internal float ThrusterForce
        {
            set => thrusterForce = value;
        }

        [SerializeField]
        private float thrusterDeteriorateRate;

        [SerializeField]
        private float forwardFrictionModifier = 1;

        [SerializeField]
        private float sidewaysFrictionModifier = 1;

        [Space]
        [Header("Rotation Clamping")]
        [SerializeField]
        private float maxPitch = 45f; // Max pitch angle in degrees

        [SerializeField]
        private float maxRoll = 30f; // Max roll angle in degrees

        [Header("Dampening")]
        [SerializeField]
        private float dampeningThreshold = 10f; // Start dampening when 10 degrees away from max

        [SerializeField]
        private float dampeningAmount = 3f;

        [Space]
        [Header("Wheels")]
        [SerializeField]
        private WheelSet wheels;

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            foreach (Wheel wheel in wheels.Values)
            {
                wheel.SetForwardFrictionModifier(forwardFrictionModifier);
                wheel.SetSidewaysFrictionModifier(sidewaysFrictionModifier);
            }
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
            HandleBrake(brake);
            HandleSteer(steer);
        }

        private void Reset()
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            throttle = 0;
            steer = 0;
            brake = false;
            thruster = 0;

            foreach (Wheel wheel in wheels.Values)
            {
                wheel.Reset();
            }
        }

        internal void Respawn(Transform transform)
        {
            rb.transform.position = transform.position;
            rb.transform.rotation = transform.rotation;
            Reset();
        }

        private void HandleThruster()
        {
            Vector3 thrusterForce = thruster * actualThruster * Vector3.up;
            rb.AddForce(thrusterForce, ForceMode.Force);
        }

        private void HandleMotor(float throttle)
        {
            float motorInput = throttle * motorForce;
            wheels[WheelLayout.FrontLeft].ApplyMotor(motorInput);
            wheels[WheelLayout.FrontRight].ApplyMotor(motorInput);
            wheels[WheelLayout.RearLeft].ApplyMotor(motorInput);
            wheels[WheelLayout.RearRight].ApplyMotor(motorInput);
        }

        private void HandleBrake(bool brake)
        {
            float brakeInput = brake ? brakeForce : 0;
            wheels[WheelLayout.FrontLeft].ApplyBrake(brakeInput);
            wheels[WheelLayout.FrontRight].ApplyBrake(brakeInput);
            wheels[WheelLayout.RearLeft].ApplyBrake(brakeInput);
            wheels[WheelLayout.RearRight].ApplyBrake(brakeInput);
        }

        private void HandleSteer(float steer)
        {
            if (wheels.Values.All(wheel => !wheel.Grounded))
            {
                // Debug.LogWarning("Flying");
                Vector3 steerInput = steer * flightSteerPropulsion * rb.transform.right;
                rb.AddRelativeTorque(steerInput, ForceMode.Force);
            }
            else
            {
                // Debug.LogWarning("Driving");
                float steerInput = steerAngle * steer;
                wheels[WheelLayout.FrontLeft].ApplySteer(steerInput);
                wheels[WheelLayout.FrontRight].ApplySteer(steerInput);
            }
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
                thrusterBurnDuration += Time.fixedDeltaTime;
                actualThruster -= thrusterBurnDuration * thrusterDeteriorateRate;
                yield return new WaitForFixedUpdate();
            }
        }

        private void ClampRotation()
        {
            Vector3 eulerRotation = transform.eulerAngles;

            // Ensure angles are between 0 and 360
            if (eulerRotation.x > 180)
                eulerRotation.x -= 360;
            if (eulerRotation.z > 180)
                eulerRotation.z -= 360;

            eulerRotation.x = Mathf.Clamp(eulerRotation.x, -maxPitch, maxPitch);
            eulerRotation.z = Mathf.Clamp(eulerRotation.z, -maxRoll, maxRoll);

            transform.eulerAngles = eulerRotation;

            // Debug.Log("Roll Amount: " + eulerRotation.z);
        }

        private void DampenRotation()
        {
            Vector3 eulerRotation = transform.eulerAngles;

            float pitchDampening = Mathf.InverseLerp(
                maxPitch - dampeningThreshold,
                maxPitch,
                eulerRotation.x
            );
            float rollDampening = Mathf.InverseLerp(
                maxRoll - dampeningThreshold,
                maxRoll,
                eulerRotation.z
            );

            Vector3 angularVelocity = rb.angularVelocity;
            angularVelocity.x *= pitchDampening / dampeningAmount;
            angularVelocity.z *= rollDampening / dampeningAmount;
            rb.angularVelocity = angularVelocity;
            //Debug.Log(
            //    "Roll Dampening: " + rollDampening + ", AngularVelocity: " + angularVelocity.z
            //);
        }
    }
}
