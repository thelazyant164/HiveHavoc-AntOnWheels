using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Gameplay
{
    public sealed class GateTimer : MonoBehaviour
    {
        [Space]
        [Header("Gameplay")]
        [SerializeField]
        private float countdownTime;

        [SerializeField]
        private Transform gate;

        [Space]
        [Header("Movement")]
        [SerializeField]
        private Transform open;

        [SerializeField]
        private Transform close;

        [Header("SFX")]
        [SerializeField]
        private AudioSource gateAmbienceAudio;

        [SerializeField]
        private AudioSource gateFoleyAudio;

        [SerializeField]
        private AudioClip gateStart;

        [SerializeField]
        private AudioClip gateClose;

        [SerializeField]
        private float gateCloseSFXDuration;
        private bool playedGateClose = false;

        internal event EventHandler OnClose;

        private void Awake()
        {
            gate.position = open.position;
        }

        private void Start()
        {
            GameManager.Instance.RegisterGate(this);
        }

        internal void StartTimer() => StartCoroutine(CloseGate(countdownTime));

        private IEnumerator CloseGate(float countdownTime)
        {
            gateAmbienceAudio.Play();
            gateFoleyAudio.PlayOneShot(gateStart);
            float timeRemaining = countdownTime;
            while (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                gate.position = Vector3.Lerp(
                    open.position,
                    close.position,
                    (countdownTime - timeRemaining) / countdownTime
                );
                if (!playedGateClose && timeRemaining <= gateCloseSFXDuration)
                {
                    gateFoleyAudio.PlayOneShot(gateClose);
                    playedGateClose = true;
                }
                yield return new WaitForEndOfFrame();
            }
            gate.position = close.position;
            OnClose?.Invoke(this, EventArgs.Empty);
            gateAmbienceAudio.Stop();
        }
    }
}
