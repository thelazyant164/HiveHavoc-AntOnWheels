using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Camera;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Combat;
using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI
{
    public sealed class ShootIndicator : MonoBehaviour, IServiceConsumer<ITrackableHostile>
    {
        [SerializeField]
        private Image shootHotspot;
        private Coroutine trackTarget;
        private RectTransform rect;

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
            StartCoroutine(Spin());
            shootHotspot.gameObject.SetActive(false);
        }

        private void Start()
        {
            Register(ServiceManager.Instance.HotspotHighlightService);
        }

        public void Register(IServiceProvider<ITrackableHostile> serviceProvider)
        {
            serviceProvider.OnAvailable += (object sender, ITrackableHostile shootableTarget) =>
            {
                shootableTarget.OnStartTracking += (object sender, EventArgs e) =>
                {
                    if (trackTarget != null)
                    {
                        StopCoroutine(trackTarget);
                    }
                    shootHotspot.gameObject.SetActive(true);
                    trackTarget = StartCoroutine(TrackTarget(shootableTarget));
                };
                shootableTarget.OnStopTracking += (object sender, EventArgs e) =>
                {
                    StopCoroutine(trackTarget);
                    shootHotspot.gameObject.SetActive(false);
                };
            };
        }

        private IEnumerator Spin()
        {
            while (true)
            {
                shootHotspot.rectTransform.Rotate(Vector3.forward, -1f);
                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator TrackTarget(ITrackableHostile hostile)
        {
            UnityEngine.Camera shooterCamera = CameraManager.Instance[Role.Shooter].Camera;

            float offsetX = 400; // MAGIC NUMBER - DOES NOT COMPREHEND

            while (true)
            {
                shootHotspot.gameObject.SetActive(true);
                Vector3 viewportPoint = shooterCamera.WorldToViewportPoint(hostile.WorldPosition);

                if (!IsVisible(viewportPoint))
                {
                    shootHotspot.gameObject.SetActive(false);
                }
                else
                {
                    rect.anchoredPosition = new Vector2(
                        viewportPoint.x * shooterCamera.scaledPixelWidth + offsetX,
                        viewportPoint.y * shooterCamera.scaledPixelHeight
                    );
                }
                yield return new WaitForEndOfFrame();
            }
        }

        private bool IsVisible(Vector3 viewportPoint)
        {
            bool inRangeX = (0 <= viewportPoint.x && viewportPoint.x <= 1);
            bool inRangeY = (0 <= viewportPoint.y && viewportPoint.y <= 1);
            bool inRangeZ = 0 <= viewportPoint.z;
            return inRangeX && inRangeY && inRangeZ;
        }
    }
}
