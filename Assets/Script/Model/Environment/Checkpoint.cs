using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Gameplay
{
    public enum ReconstructibleDynamic
    {
        Destructible,
        Movable
    }

    public struct ReconstructibleData
    {
        private ReconstructibleDynamic type;

        private Vector3 position;
        private Quaternion rotation;
        private GameObject obj;

        private ReconstructibleData(GameObject obj, ReconstructibleDynamic type)
        {
            this.type = type;
            this.obj =
                type is ReconstructibleDynamic.Movable
                    ? null
                    : GameObject.Instantiate(obj, obj.transform.parent);
            this.obj?.SetActive(false);
            position = obj.transform.position;
            rotation = obj.transform.rotation;
        }

        public GameObject Reconstruct(GameObject obj = null)
        {
            if (obj == null && type is ReconstructibleDynamic.Destructible)
            {
                GameObject reconstructed = GameObject.Instantiate(
                    this.obj,
                    this.obj.transform.parent
                );
                reconstructed.SetActive(true);
                return reconstructed;
            }
            else
            {
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                return obj;
            }
        }

        public static ReconstructibleData FromDynamic(GameObject obj)
        {
            if (obj.TryGetComponent(out IMovable movable))
            {
                return new ReconstructibleData(obj, ReconstructibleDynamic.Movable);
            }
            else if (obj.TryGetComponent(out IDestructible destructible))
            {
                return new ReconstructibleData(obj, ReconstructibleDynamic.Destructible);
            }
            return new ReconstructibleData(obj, ReconstructibleDynamic.Movable);
        }
    }

    [RequireComponent(typeof(Collider))]
    public sealed class Checkpoint : MonoBehaviour, ITrigger<Checkpoint>
    {
        [SerializeField]
        private LayerMask receptible;
        public LayerMask Receptible => receptible;

        [SerializeField]
        private List<GameObject> memoizeDynamic;
        private Dictionary<GameObject, ReconstructibleData> dynamicCache = new();

        public event EventHandler OnTrigger;
        public event EventHandler OnTerminate;

        private void Awake()
        {
            CacheDynamic();
            OnTrigger += ResetDynamic;
        }

        private void Start()
        {
            CheckpointManager.Instance.RegisterCheckpoint(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.InLayerMask(receptible))
            {
                OnTrigger?.Invoke(this, EventArgs.Empty);
            }
        }

        private void CacheDynamic()
        {
            foreach (GameObject dynamic in memoizeDynamic)
            {
                dynamicCache[dynamic] = ReconstructibleData.FromDynamic(dynamic);
            }
        }

        private void ResetDynamic(object sender, EventArgs e)
        {
            for (int i = 0; i < memoizeDynamic.Count; i++)
            {
                GameObject dynamic = memoizeDynamic[i];
                if (dynamic == null)
                {
                    dynamicCache[dynamic].Reconstruct();
                }
                else
                {
                    dynamicCache[dynamic].Reconstruct(dynamic);
                }
            }
        }
    }
}
