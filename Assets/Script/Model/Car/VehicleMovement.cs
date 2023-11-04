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
        public PlayerInputs input;
        public VehicleSpeed speedData;
        public CpWheelData wheelData;

        public float accelerateAxis;
        public float brakingAxis;

        private Rigidbody rigid;
        public Rigidbody rb => rigid;
        public Vector3 averageColliderSurfaceNormal;

        [Header("Scripted event modifier")]
        [SerializeField]
        private float speedModifier = 1;
        internal float SpeedModifier
        {
            get => speedModifier;
            set => speedModifier = value;
        }

        [SerializeField]
        private float steerModifier = 1;
        internal float SteerModifier
        {
            get => steerModifier;
            set => steerModifier = value;
        }

        [SerializeField]
        private float thrusterModifer = 1;
        internal float ThrusterModifier
        {
            get => thrusterModifer;
            set => thrusterModifer = value;
        }

        [SerializeField]
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
            rigid = GetComponentInChildren<Rigidbody>();
            Assert.IsNotNull(rb);
        }

        private void Start()
        {
            GameManager.Instance.RegisterVehicle(this);
            Driver driver = PlayerManager.Instance.Driver;
            driver.OnAccelerate += (object sender, float accelInput) =>
                input.accelInput = input.brake ? 0f : accelInput;
            driver.OnSteer += (object sender, float steerInput) =>
                input.steeringInput = input.brake ? 0f : steerInput;
            driver.OnBrake += (object sender, bool brake) => input.brake = brake;

            Shooter.Shooter shooter = PlayerManager.Instance.Shooter;
            shooter.OnThruster += (object sender, float thruster) => input.thrusterInput = thruster;
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

        internal void Reset()
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            input.Reset();
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
    }

    [Serializable]
    public class PlayerInputs
    {
        public float accelInput;
        public float steeringInput;
        public bool brake;
        public float thrusterInput;

        public void Reset()
        {
            accelInput = 0;
            steeringInput = 0;
            brake = false;
            thrusterInput = 0;
        }
    }
}
