using Cinemachine;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Respawn;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.ScriptedEvent
{
    public sealed class ResetThruster : ScriptedEvent<Checkpoint>
    {
        [SerializeField]
        private float overrideThrusterForce;

        protected override void TriggerCallback()
        {
            GameManager.Instance.Vehicle.ThrusterModifier = overrideThrusterForce;
        }
    }
}
