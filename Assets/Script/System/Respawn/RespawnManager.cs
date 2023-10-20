using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Respawn
{
    public sealed class RespawnManager : Singleton<RespawnManager>, IServiceProvider<RespawnTrigger>
    {
        private Dictionary<GameObject, RespawnableData> dynamicCache = new();
        public event EventHandler<RespawnTrigger> OnAvailable;
        public event EventHandler<GameObject> OnRespawn;

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
        }

        public void Register(RespawnTrigger service)
        {
            foreach (GameObject dynamic in service.Respawnables)
            {
                Cache(dynamic);
            }
            service.OnTrigger += (object sender, EventArgs e) => ResetAll(service.Respawnables);
            OnAvailable?.Invoke(this, service);
        }

        private void Cache(GameObject dynamic)
        {
            if (!dynamic.TryGetComponent(out IRespawnable respawnable))
            {
                Debug.LogError($"Trying to cache {dynamic}, which is not marked as dynamic!");
                return;
            }
            if (dynamicCache.ContainsKey(dynamic))
                return;
            dynamicCache[dynamic] = RespawnableData.From(respawnable);
        }

        private void ResetAll(List<GameObject> dynamics)
        {
            foreach (GameObject dynamicObj in dynamics)
            {
                GameObject respawned;
                if (dynamicObj == null)
                {
                    respawned = dynamicCache[dynamicObj].Respawn();
                }
                else
                {
                    respawned = dynamicCache[dynamicObj].Respawn(
                        dynamicObj.GetComponent<IRespawnable>()
                    );
                }
                if (respawned != dynamicObj)
                    OnRespawn?.Invoke(dynamicObj, respawned);
            }
        }
    }
}
