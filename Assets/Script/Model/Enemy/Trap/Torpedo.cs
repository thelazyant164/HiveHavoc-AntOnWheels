using System.Collections;
using System.Collections.Generic;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Projectile;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Environment;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Fluid;
using System.Linq;
using UnityEngine;
using System;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class Torpedo : Explosive, IProjectile, IPrimedExplosive<Torpedo>, IFloatable
    {
        public Rigidbody Rigidbody => Rb;

        [SerializeField]
        private LayerMask interceptedBy;
        public LayerMask InterceptedBy => interceptedBy;

        [SerializeField]
        private float velocity;
        public float Velocity => velocity;

        [SerializeField]
        private float countdown;
        public float Countdown => countdown;
        public new IDamaging.Target TargetType => IDamaging.Target.Player;
        public new Explosion<Torpedo> Explosion => new Explosion<Torpedo>(this, transform.position);

        public IFluidBody Fluid { get; private set; }

        public float Volume { get; private set; }

        [SerializeField]
        private float density;
        public float Density => density;

        [SerializeField]
        private float ratioToBoundVolume;
        public float RatioToBoundVolume => ratioToBoundVolume;

        public float SubmergedDimensionDepth { get; private set; }

        public new event EventHandler<Torpedo> OnDestroy;

        protected override void Awake()
        {
            base.Awake();
            BeginCountdown();
            OnDestroy?.Invoke(this, this); // silence warning
        }

        private void FixedUpdate()
        {
            if (Rigidbody == null || Fluid == null)
                return;
            Vector3 buoyancyForce = new Vector3(0, GetBuoyancyForce(), 0);
            Rigidbody.AddForce(buoyancyForce);
            Rigidbody.AddForce(GetBuoyancyDrag(), ForceMode.VelocityChange);
            Rigidbody.AddTorque(GetBuoyancyAngularDrag(), ForceMode.VelocityChange);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.InLayerMask(InterceptedBy))
            {
                //Debug.LogWarning($"Pollen burst exploded on contact with {collision.gameObject}");
                Destroy();
            }
        }

        public override void Explode(object sender, EventArgs e)
        {
            OnExplode -= Explode;

            IEnumerable<IDynamic> dynamicEntities = GetAffectedEntityInBlastZone();
            foreach (IDynamic entity in dynamicEntities)
            {
                if (entity is IDamageable damageable)
                {
                    damageable.TakeDamage<Torpedo>(Explosion);
                }
                if (entity is IDestructible destructible)
                {
                    destructible.Destroy();
                }
                if (entity is IMovable movable)
                {
                    movable.ReactTo(Explosion);
                }
            }
            DebugExtension.DrawWireSphere(transform.position, BlastRadius, Color.red, 1f);
            //gameObject.SetTimeOut(1f, () => Destroy(gameObject)); // destroys self after .5f -> play explosion animation?
            Destroy(gameObject);
        }

        public void Launch(Vector3 spatialImpulse) =>
            Rigidbody.AddForce(spatialImpulse, ForceMode.Impulse);

        public void BeginCountdown()
        {
            gameObject.SetTimeOut(
                countdown,
                () =>
                {
                    //Debug.LogWarning("Torpedo expired");
                    Explode();
                }
            );
        }

        public void Enter(IFluidBody fluid)
        {
            Fluid = fluid;
        }

        public void Exit(IFluidBody fluid)
        {
            Fluid = null;
        }

        public float GetSubmergedVolume()
        {
            float surfaceHeight = Fluid.SampleSurfaceHeight(transform.position) ?? 0;
            // TODO: issue: surface height keeps increasing - due to honey material config, adjust this
            //Debug.Log($"Surface height is {surfaceHeight}");
            float worldSpaceY = transform.position.y;
            //Debug.Log($"World space Y is {worldSpaceY}");
            float submergedDepth = surfaceHeight - worldSpaceY;
            //Debug.Log($"Submerged depth is {submergedDepth}");
            float unclampedSubmergedRatio = submergedDepth / SubmergedDimensionDepth;
            //Debug.Log($"Unclamped submerged ratio is {unclampedSubmergedRatio}");
            float clampedSubmergedRatio = Mathf.Clamp01(unclampedSubmergedRatio);
            //Debug.Log($"Clamped submerged ratio is {clampedSubmergedRatio}");
            float roughSubmergedVolume = clampedSubmergedRatio * Volume;
            //Debug.Log($"Submerged volume is {roughSubmergedVolume}");
            return roughSubmergedVolume;
        }

        public float GetBuoyancyForce() =>
            Mathf.Abs(Physics.gravity.y) * Fluid.Density * GetSubmergedVolume();

        public Vector3 GetBuoyancyDrag() =>
            GetSubmergedVolume() * (-Rigidbody.velocity) * Fluid.Drag * Time.fixedDeltaTime;

        public Vector3 GetBuoyancyAngularDrag() =>
            GetSubmergedVolume()
            * (-Rigidbody.angularVelocity)
            * Fluid.AngularDrag
            * Time.fixedDeltaTime;

        public void ResetMass()
        {
            Collider floatingCollider = GetComponentInChildren<Collider>();
            Volume = floatingCollider.GetBoundVolume() * ratioToBoundVolume;
            Rigidbody.mass = density * Volume;

            SubmergedDimensionDepth = floatingCollider.bounds.size.y;
            return;
        }
    }
}
