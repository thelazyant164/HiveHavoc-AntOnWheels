using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
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

        [SerializeField]
        private float thrusterForce;
        internal float ThrusterForce
        {
            set => thrusterForce = value;
        }

        [SerializeField]
        private float thrusterDeteriorateRate;

        public PlayerInputs input;
        public VehicleSpeed speedData;
        public CpWheelData wheelData;

        public float accelerateAxis;
        public float brakingAxis;

        [HideInInspector]
        public Rigidbody rb;
        public Vector3 averageColliderSurfaceNormal;

        [Header("Scripted event modifier")]
        [SerializeField]
        private float speedModifier = 1;
        internal float SpeedModifier
        {
            get => speedModifier;
            set => speedModifier = value;
        }
        private float airLinearDragModifier = 1;
        internal float AirLinearDragModifier
        {
            get => airLinearDragModifier;
            set => airLinearDragModifier = value;
        }

        private bool prevGroundedState;
        public static event Action<VehicleMovement> OnLeavingGround = vehicleMovement => { };
        public static event Action<VehicleMovement> OnLanding = vehicleMovement => { };

        private void Awake()
        {
            rb = GetComponentInChildren<Rigidbody>();
            Assert.IsNotNull(rb);
            StartCoroutine(ThrusterDeteriorate());
        }

        private void Start()
        {
            GameManager.Instance.RegisterVehicle(this);
            Driver driver = PlayerManager.Instance.Driver;
            driver.OnAccelerate += (object sender, float input) => throttle = brake ? 0f : input;
            driver.OnSteer += (object sender, float input) => steer = brake ? 0f : input;
            driver.OnBrake += (object sender, bool brake) => this.brake = brake;

            Shooter.Shooter shooter = PlayerManager.Instance.Shooter;
            shooter.OnThruster += (object sender, float thruster) => this.thruster = thruster;
        }

        private void Update()
        {
            // Debug.Log($"Acceleration Input: {input.accelInput}, Steering Input: {input.steeringInput}");
            if (prevGroundedState == false && wheelData.grounded)
            {
                OnLanding(this);
            }
            else if (prevGroundedState == true && !wheelData.grounded)
            {
                OnLeavingGround(this);
            }

            prevGroundedState = wheelData.grounded;
        }

        private void FixedUpdate()
        {
            HandleInputs();
            HandleThruster();
        }

        private void Reset()
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            throttle = 0;
            steer = 0;
            brake = false;
            thruster = 0;
        }

        private void HandleInputs()
        {
            // Set accelInput and steeringInput based on throttle and steer values
            input.accelInput = throttle;
            input.steeringInput = steer;
            input.brake = brake;
            // Debug.Log($"input.steeringInput: {input.steeringInput}, steer: {steer}");
        }

        private void HandleThruster()
        {
            Vector3 thrusterForce = thruster * actualThruster * Vector3.up;
            rb.AddForce(thrusterForce, ForceMode.Force);
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

        internal void Respawn(Transform transform)
        {
            // ASSUMPTION: transform of VehicleMovement directs all vehicle physics (i.e. VehicleMovement is top-level parent)
            this.transform.SetPositionAndRotation(transform.position, transform.rotation);
            Physics.SyncTransforms();
            Reset();
        }

        internal void ApplyRecoil(Vector3 recoilImpulse) =>
            rb.AddForce(recoilImpulse, ForceMode.Impulse);

        internal bool IsMovingGrounded => wheelData.grounded && speedData.speed > 0;
    }

    [Serializable]
    public class PlayerInputs
    {
        public float accelInput;
        public float steeringInput;
        public bool brake;
    }
}
