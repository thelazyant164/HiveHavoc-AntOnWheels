using System.Collections;
using System.Collections.Generic;
using static Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Camera.SplitConfiguration;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public sealed class SplitController : MonoBehaviour
    {
        [SerializeField]
        private bool hasUI = false;

        [SerializeField, ShowWhen("hasUI", true)]
        private UnityEngine.Camera UICamera;

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

        internal void AdjustTo(Split newSplit, float time)
        {
            if (hasUI)
            {
                UICamera?.gameObject.SetActive(newSplit.weight != 0);
            }
            StartCoroutine(AdaptTo(newSplit, time));
        }

        private IEnumerator AdaptTo(Split newSplit, float time)
        {
            Adjusting = true;
            float timeElapsed = 0;
            Rect current = camera.rect;
            Rect target = newSplit.CameraRect;
            while (timeElapsed < time)
            {
                timeElapsed += Time.unscaledDeltaTime;
                camera.rect = RectExtension.Lerp(current, target, timeElapsed / time);
                yield return null;
            }
            camera.rect = target;
            Adjusting = false;
        }
    }
}
