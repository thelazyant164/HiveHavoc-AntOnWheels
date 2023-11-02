using System.Collections;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Driver
{
    public sealed class VehicleThruster : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve thrusterForceCurve;

        [SerializeField]
        private float airborneDuration = 0;

        [SerializeField]
        private float sampledThruster = 0;

        [SerializeField]
        private float appliedThruster = 0;

        private VehicleMovement vehicle;

        private void Awake()
        {
            vehicle = GetComponent<VehicleMovement>();
            StartCoroutine(CheckGrounded());
        }

        private void FixedUpdate()
        {
            ApplyThruster(vehicle.input.thrusterInput, vehicle.ThrusterModifier);
        }

        private IEnumerator CheckGrounded()
        {
            while (true)
            {
                airborneDuration += Time.deltaTime;
                if (vehicle.wheelData.grounded)
                {
                    airborneDuration = 0;
                }
                yield return new WaitForFixedUpdate();
            }
        }

        private void ApplyThruster(float input, float modifier = 1)
        {
            sampledThruster = thrusterForceCurve.Evaluate(airborneDuration);
            appliedThruster = sampledThruster * input * modifier;
            Vector3 thrusterForce = appliedThruster * Vector3.up;
            vehicle.rb.AddForce(thrusterForce);
        }
    }
}
