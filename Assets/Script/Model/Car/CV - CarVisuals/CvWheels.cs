using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver
{
    public class CvWheels : MonoBehaviour
    {
        private Rigidbody carRb;
        private Transform attachPoint;
        private PlayerInputs input;
        private VehicleSpeed vehicleSpeed;
        public bool grounded;

        public LayerMask groundLayer;

        [Header("Wheel Dimensions")]
        public WheelPosition wheelPosition;
        public float wheelRadius;
        public float suspensionMaxHeight;
        public float frontWheelTurnMaxAngle = 30;

        private void Awake()
        {
            attachPoint = transform.parent;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetUpWheel(Rigidbody carRb)
        {
            this.carRb = carRb;
            wheelPosition = transform.parent.localPosition.z > 0 ? WheelPosition.Front : WheelPosition.Back;
        }

        public void ProcessWheelVisuals(PlayerInputs input, VehicleSpeed vehicleSpeed)
        {
            this.input = input;
            this.vehicleSpeed = vehicleSpeed;
            UpdateWheelPosition();
            UpdateWheelRotations();
        }

        private void UpdateWheelPosition()
        {
            RaycastHit hit;

            grounded = Physics.Raycast(attachPoint.position, -attachPoint.up, out hit, suspensionMaxHeight);

            float wheelExtension = suspensionMaxHeight;

            if (grounded)
            {
                wheelExtension = hit.distance;
            }

            transform.position = attachPoint.position + ((wheelExtension - wheelRadius) * -attachPoint.up);
        }

        private void UpdateWheelRotations()
        {
            //In case tyre model is flipped
            int rotationDirection = transform.parent.localScale.x > 0 ? 1 : -1;

            //Front wheel steering - Checks if the wheel is in front of the cars center of mass
            if (wheelPosition == WheelPosition.Front)
                transform.localRotation = Quaternion.Euler(0, (input.steeringInput * frontWheelTurnMaxAngle * rotationDirection), 0);

            //Converts linear speed into angular speed and applies it to the wheels
            float wheelCircumference = 2 * Mathf.PI * (wheelRadius / 2);
            float angularVelocity = vehicleSpeed.forwardSpeed / wheelCircumference /** Time.deltaTime*/;
            //transform.GetChild(0).Rotate(0, angularVelocity, 0);
            transform.GetChild(0).Rotate(angularVelocity, 0, 0);
        }
    }

    public enum WheelPosition { Front, Back, Other }
}