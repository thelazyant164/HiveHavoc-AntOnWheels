using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Respawn;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Shooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels
{
    public sealed class ServiceManager : Singleton<ServiceManager>
    {
        internal AimService AimService { get; private set; }
        internal ReloadService ReloadService { get; private set; }
        internal HotspotHighlightService HotspotHighlightService { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError(
                    "There's more than one InputManager! " + transform + " - " + Instance
                );
                Destroy(gameObject);
                return;
            }
            Instance = this;

            AimService = GetComponentInChildren<AimService>();
            ReloadService = GetComponentInChildren<ReloadService>();
            HotspotHighlightService = GetComponentInChildren<HotspotHighlightService>();
        }
    }
}
