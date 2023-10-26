using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Camera
{
    public enum SplitDirection
    {
        Horizontal,
        Vertical
    }

    public enum SplitSide
    {
        Left,
        Right,
        Top,
        Bottom
    }

    public struct SplitConfiguration
    {
        public struct Split
        {
            public SplitSide side;
            public float weight;

            private SplitSide Reorient
            {
                get
                {
                    switch (side)
                    {
                        case SplitSide.Left:
                            return SplitSide.Bottom;
                        case SplitSide.Right:
                            return SplitSide.Top;
                        case SplitSide.Top:
                            return SplitSide.Right;
                        case SplitSide.Bottom:
                            return SplitSide.Left;
                        default:
                            throw new System.Exception($"Invalid split side: encountered {side}");
                    }
                }
            }

            private SplitSide Opposite
            {
                get
                {
                    switch (side)
                    {
                        case SplitSide.Left:
                            return SplitSide.Right;
                        case SplitSide.Right:
                            return SplitSide.Left;
                        case SplitSide.Top:
                            return SplitSide.Bottom;
                        case SplitSide.Bottom:
                            return SplitSide.Top;
                        default:
                            throw new System.Exception($"Invalid split side: encountered {side}");
                    }
                }
            }

            public Rect CameraRect
            {
                get
                {
                    switch (side)
                    {
                        case SplitSide.Left:
                            return new Rect(0, 0, weight, 1);
                        case SplitSide.Right:
                            return new Rect(1 - weight, 0, weight, 1);
                        case SplitSide.Top:
                            return new Rect(0, 1 - weight, 1, weight);
                        case SplitSide.Bottom:
                            return new Rect(0, 0, 1, weight);
                        default:
                            throw new System.Exception($"Invalid split side: encountered {side}");
                    }
                }
            }

            public Split(SplitSide side, float weight)
            {
                if (weight < 0 || weight > 1f)
                    throw new System.Exception(
                        $"Invalid weight when creating split: encountered {weight}"
                    );
                this.side = side;
                this.weight = weight;
            }

            public static Split operator -(Split split) =>
                new Split(split.Opposite, 1 - split.weight);

            public static Split operator !(Split split) => new Split(split.Reorient, split.weight);
        }

        public SplitDirection direction;
        private readonly Dictionary<Role, Split> split;

        public Split this[Role key] => split[key];

        public SplitConfiguration(Role role, Split split) : this(role, split.side, split.weight) { }

        public SplitConfiguration(Role role, SplitSide side, float weight)
        {
            split = new();
            switch (side)
            {
                case SplitSide.Left
                or SplitSide.Right:
                    direction = SplitDirection.Horizontal;
                    break;
                case SplitSide.Top
                or SplitSide.Bottom:
                    direction = SplitDirection.Vertical;
                    break;
                default:
                    throw new System.Exception($"Invalid split: encountered {side}");
            }
            Split current = new Split(side, weight);
            split.Add(role, current);
            split.Add(role == Role.Driver ? Role.Shooter : Role.Driver, -current);
        }

        public SplitConfiguration Collapse(Role role) =>
            new SplitConfiguration(role, this[role].side, 0);

        public SplitConfiguration ChangeOrientation(Role role) =>
            new SplitConfiguration(role, !this[role]);

        public static SplitConfiguration HorizontalEven =>
            new SplitConfiguration(Role.Driver, SplitSide.Left, .5f);

        public static SplitConfiguration VerticalEven =>
            new SplitConfiguration(Role.Driver, SplitSide.Bottom, .5f);

        public static SplitConfiguration HorizontalDriverEmphasis =>
            new SplitConfiguration(Role.Driver, SplitSide.Left, .7f);

        public static SplitConfiguration VerticalShooterEmphasis =>
            new SplitConfiguration(Role.Driver, SplitSide.Bottom, .3f);

        public static SplitConfiguration VerticalDriverOnly =>
            new SplitConfiguration(Role.Driver, SplitSide.Bottom, 1f);

        public static SplitConfiguration HorizontalDriverOnly =>
            new SplitConfiguration(Role.Driver, SplitSide.Left, 1f);

        public static SplitConfiguration VerticalShooterOnly =>
            new SplitConfiguration(Role.Driver, SplitSide.Bottom, 0);

        public static SplitConfiguration HorizontalShooterOnly =>
            new SplitConfiguration(Role.Driver, SplitSide.Left, 0);
    }

    public sealed class SplitManager : Singleton<SplitManager>
    {
        [SerializeField]
        private float splitAdjustTime = .5f;
        private Coroutine splitAdjustment;
        private SplitConfiguration? currentSplit = null;
        private Dictionary<Role, SplitController> controllers = new();

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

        internal void Register(SplitController controller, Role role)
        {
            if (controllers.TryAdd(role, controller))
            {
                return;
            }
            Debug.LogError($"More than 1 SplitController exists for role {role}");
        }

        // Some variables to tweak in tandem:
        // - Camera FOV (fine at 75 when horizontal, needs smaller ~50 when vertical)
        // - Viewport rect (normalized to 1, so should math it out)

        internal void Apply(SplitConfiguration config)
        {
            if (splitAdjustment == null)
            {
                splitAdjustment = StartCoroutine(ApplySplit(config));
            }
        }

        private IEnumerator ApplySplit(SplitConfiguration config)
        {
            if (currentSplit is SplitConfiguration split && split.direction != config.direction)
            {
                yield return SwitchOrientation(split, Role.Shooter);
            }
            yield return AdjustSplit(config, splitAdjustTime);
            currentSplit = config;
            splitAdjustment = null;
        }

        private IEnumerator AdjustSplit(SplitConfiguration config, float time = 0)
        {
            foreach (var controller in controllers)
            {
                controller.Value.AdjustTo(config[controller.Key], time);
            }
            yield return new WaitUntil(
                () => controllers.Values.All(controller => !controller.Adjusting)
            );
        }

        private IEnumerator SwitchOrientation(SplitConfiguration current, Role role)
        {
            current = current.Collapse(role);
            yield return AdjustSplit(current, splitAdjustTime);
            current = current.ChangeOrientation(role);
            yield return AdjustSplit(current);
        }
    }
}
