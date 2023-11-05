using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Respawn;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI
{
    public sealed class CheckpointActivated : MonoBehaviour
    {
        [Header("UI indicator")]
        [SerializeField]
        private float moveDuration;

        [SerializeField]
        private float visibleDuration;

        [SerializeField]
        private RectTransform hiddenPosition;
        private Vector2 Hidden => hiddenPosition.anchoredPosition;

        [SerializeField]
        private RectTransform visiblePosition;
        private Vector2 Visible => visiblePosition.anchoredPosition;

        private Coroutine showing;

        [SerializeField]
        private RectTransform indicator;

        [Space]
        [Header("SFX")]
        [SerializeField]
        private AudioClip checkpointActivatedSFX;

        private void Awake()
        {
            Assert.IsNotNull(indicator);
            indicator.anchoredPosition = Hidden;
        }

        private void Start()
        {
            CheckpointManager.Instance.OnTrigger += OnCheckpointTrigger;
        }

        private void OnCheckpointTrigger(object sender, Checkpoint checkpoint)
        {
            if (showing != null)
            {
                StopCoroutine(showing);
                indicator.anchoredPosition = Hidden;
            }
            showing = StartCoroutine(OnCheckpointTrigger());
        }

        private IEnumerator OnCheckpointTrigger()
        {
            UIManager.Instance.UIAudio.PlayOneShot(checkpointActivatedSFX);
            yield return StartCoroutine(indicator.LerpTo(Visible, moveDuration));
            yield return new WaitForSeconds(visibleDuration);
            yield return StartCoroutine(indicator.LerpTo(Hidden, moveDuration));
        }
    }
}
