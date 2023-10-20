using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using System;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Respawn
{
    public enum RespawnableType
    {
        Destructible,
        Movable
    }

    public struct RespawnableData
    {
        private readonly RespawnableType type;
        private Vector3 position;
        private Quaternion rotation;
        private readonly IRespawnable original;
        private readonly IRespawnable template;
        private GameObject respawned;

        private RespawnableData(IRespawnable obj, RespawnableType type)
        {
            original = obj;
            this.type = type;
            template =
                type is RespawnableType.Movable
                    ? null
                    : GameObject
                        .Instantiate(original.GameObject, original.Transform.parent)
                        .GetComponent<IRespawnable>();
            template?.GameObject.SetActive(false);
            respawned = original.GameObject;
            position = original.Transform.position;
            rotation = original.Transform.rotation;
        }

        public GameObject Respawn(IRespawnable obj = null)
        {
            if (obj == null && respawned == null && type is RespawnableType.Destructible)
            {
                GameObject reconstructed = GameObject.Instantiate(
                    template.GameObject,
                    template.Transform.parent
                );
                reconstructed.SetActive(true);
                respawned = reconstructed;
            }
            else if (obj is IMovable movable)
            {
                movable.ResetTo(position, rotation);
            }
            return respawned;
        }

        public static RespawnableData From(IRespawnable obj)
        {
            if (obj.GameObject.TryGetComponent(out IDestructible destructible))
            {
                return new RespawnableData(obj, RespawnableType.Destructible);
            }
            return new RespawnableData(obj, RespawnableType.Movable);
        }
    }

    public interface IRespawnable : IDynamic
    {
        public GameObject GameObject { get; }
        public Transform Transform { get; }
    }
}
