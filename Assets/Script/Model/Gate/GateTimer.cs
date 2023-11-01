using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI;
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

        [Space]
        [Header("VFX")]
        [SerializeField]
        private ParticleSystem gateStartVFX;

        [SerializeField]
        private ParticleSystem gateCloseVFX;

        [SerializeField]
        private float gateStartVFXDelay;

        [SerializeField]
        private float gateCloseVFXDuration;

        private bool playedGateCloseVFX = false;

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
        private AudioClip announceHalfway;

        [SerializeField]
        private AudioClip announceQuarter;

        [SerializeField]
        private float gateCloseSFXDuration;

        private bool playedGateClose = false;
        private bool playedHalfway = false;
        private bool playedQuarter = false;
        private AudioSource vocalAudio;

        internal event EventHandler OnClose;

        private void Awake()
        {
            gate.position = open.position;
        }

        private void Start()
        {
            GameManager.Instance.RegisterGate(this);
            vocalAudio = UIManager.Instance.VocalAudio;
        }

        internal void StartTimer() => StartCoroutine(CloseGate(countdownTime));

        private IEnumerator CloseGate(float countdownTime)
        {
            gameObject.SetTimeOut(gateStartVFXDelay, () => gateStartVFX.Play());
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
                if (!playedHalfway && timeRemaining <= countdownTime / 2)
                {
                    vocalAudio.PlayOneShot(announceHalfway);
                    playedHalfway = true;
                }
                if (!playedQuarter && timeRemaining <= countdownTime / 4)
                {
                    vocalAudio.PlayOneShot(announceQuarter);
                    playedQuarter = true;
                }
                if (!playedGateClose && timeRemaining <= gateCloseSFXDuration)
                {
                    gateFoleyAudio.PlayOneShot(gateClose);
                    playedGateClose = true;
                }
                if (!playedGateCloseVFX && timeRemaining <= gateCloseVFXDuration)
                {
                    gateCloseVFX.Play();
                    playedGateCloseVFX = true;
                }
                yield return new WaitForEndOfFrame();
            }
            gate.position = close.position;
            OnClose?.Invoke(this, EventArgs.Empty);
            gateAmbienceAudio.Stop();
        }
    }
}
