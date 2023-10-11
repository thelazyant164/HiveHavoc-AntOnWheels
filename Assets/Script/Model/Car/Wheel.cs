using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver
{
    [RequireComponent(typeof(WheelCollider))]
    public sealed class Wheel : MonoBehaviour
    {
        private MeshFilter mesh;
        private WheelCollider wheelCollider;
        internal bool Grounded => wheelCollider.isGrounded;

        [Header("Debug")]
        [SerializeField]
        private float motor;

        [SerializeField]
        private float steer;

        [SerializeField]
        private float brake;

        private void Awake()
        {
            mesh = GetComponentInChildren<MeshFilter>();
            wheelCollider = GetComponent<WheelCollider>();
        }

        private void FixedUpdate()
        {
            wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
            mesh.transform.SetPositionAndRotation(pos, rot);

            steer = wheelCollider.steerAngle;
            motor = wheelCollider.motorTorque;
            brake = wheelCollider.brakeTorque;
        }

        internal void Reset()
        {
            wheelCollider.steerAngle = 0;
            wheelCollider.motorTorque = 0;
            wheelCollider.brakeTorque = 0;
        }

        internal void ApplySteer(float value) => wheelCollider.steerAngle = value;

        internal void ApplyMotor(float value) => wheelCollider.motorTorque = value;

        internal void ApplyBrake(float value) => wheelCollider.brakeTorque = value;

        internal void SetForwardFrictionModifier(float value = 1)
        {
            if (wheelCollider == null)
                wheelCollider = GetComponent<WheelCollider>();
            WheelFrictionCurve forwardFriction = wheelCollider.forwardFriction;
            forwardFriction.stiffness = value;
            wheelCollider.forwardFriction = forwardFriction;
        }

        internal void SetSidewaysFrictionModifier(float value = 1)
        {
            if (wheelCollider == null)
                wheelCollider = GetComponent<WheelCollider>();
            WheelFrictionCurve sidewaysFriction = wheelCollider.sidewaysFriction;
            sidewaysFriction.stiffness = value;
            wheelCollider.sidewaysFriction = sidewaysFriction;
        }
    }
}
