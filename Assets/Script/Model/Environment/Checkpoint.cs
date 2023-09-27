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
        private IDynamic obj;

        private ReconstructibleData(IDynamic obj, ReconstructibleDynamic type)
        {
            this.type = type;
            this.obj =
                type is ReconstructibleDynamic.Movable
                    ? null
                    : GameObject
                        .Instantiate(obj.GameObject, obj.Transform.parent)
                        .GetComponent<IDynamic>();
            this.obj?.GameObject.SetActive(false);
            position = obj.Transform.position;
            rotation = obj.Transform.rotation;
        }

        public IDynamic Reconstruct(IDynamic obj = null)
        {
            if (obj == null && type is ReconstructibleDynamic.Destructible)
            {
                GameObject reconstructed = GameObject.Instantiate(
                    this.obj.GameObject,
                    this.obj.Transform.parent
                );
                reconstructed.SetActive(true);
                return reconstructed.GetComponent<IDynamic>();
            }
            else if (obj is IMovable movable)
            {
                movable.ResetTo(position, rotation);
                return obj;
            }
            return obj;
        }

        public static ReconstructibleData FromDynamic(IDynamic obj)
        {
            if (obj.GameObject.TryGetComponent(out IMovable movable))
            {
                return new ReconstructibleData(movable, ReconstructibleDynamic.Movable);
            }
            else if (obj.GameObject.TryGetComponent(out IDestructible destructible))
            {
                return new ReconstructibleData(destructible, ReconstructibleDynamic.Destructible);
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

        private void OnDestroy()
        {
            OnTerminate?.Invoke(this, EventArgs.Empty);
        }

        private void CacheDynamic()
        {
            foreach (GameObject dynamicObj in memoizeDynamic)
            {
                if (!dynamicObj.TryGetComponent(out IDynamic dynamic))
                {
                    Debug.LogError(
                        $"Trying to cache {dynamicObj}, which is not marked as dynamic!"
                    );
                }
                dynamicCache[dynamicObj] = ReconstructibleData.FromDynamic(dynamic);
            }
        }

        private void ResetDynamic(object sender, EventArgs e)
        {
            for (int i = 0; i < memoizeDynamic.Count; i++)
            {
                GameObject dynamicObj = memoizeDynamic[i];
                if (dynamicObj == null)
                {
                    dynamicCache[dynamicObj].Reconstruct();
                }
                else
                {
                    dynamicCache[dynamicObj].Reconstruct(dynamicObj.GetComponent<IDynamic>());
                }
            }
        }
    }
}
