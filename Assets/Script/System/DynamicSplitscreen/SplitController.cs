using System.Collections;
using System.Collections.Generic;
using static Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Camera.SplitConfiguration;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public sealed class SplitController : MonoBehaviour
    {
        private new UnityEngine.Camera camera;

        [SerializeField]
        private Role role;
        internal bool Adjusting { get; private set; }

        private void Awake()
        {
            camera = GetComponent<UnityEngine.Camera>();
        }

        private void Start()
        {
            SplitManager.Instance.Register(this, role);
        }

        internal void AdjustTo(Split newSplit, float time) =>
            StartCoroutine(AdaptTo(newSplit, time));

        private IEnumerator AdaptTo(Split newSplit, float time)
        {
            Adjusting = true;
            float timeElapsed = 0;
            Rect current = camera.rect;
            Rect target = newSplit.CameraRect;
            while (timeElapsed < time)
            {
                timeElapsed += Time.deltaTime;
                camera.rect = Lerp(current, target, timeElapsed / time);
                yield return null;
            }
            camera.rect = target;
            Adjusting = false;
        }

        private static Rect Lerp(Rect start, Rect target, float ratio)
        {
            float x = Mathf.Lerp(start.x, target.x, ratio);
            float y = Mathf.Lerp(start.y, target.y, ratio);
            float width = Mathf.Lerp(start.width, target.width, ratio);
            float height = Mathf.Lerp(start.height, target.height, ratio);
            return new Rect(x, y, width, height);
        }
    }
}
