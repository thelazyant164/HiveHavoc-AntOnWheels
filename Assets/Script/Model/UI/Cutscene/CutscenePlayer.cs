using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.Video;
using System;
using UnityEngine.SceneManagement;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Cutscene
{
    public sealed class CutscenePlayer : MonoBehaviour, ICutscenePlayer
    {
        private VideoPlayer player;

        [SerializeField]
        private bool transitionSceneWhenDone = false;

        [SerializeField, ShowWhen("transitionSceneWhenDone", true)]
        private string nextScene;

        [SerializeReference, ShowWhen("transitionSceneWhenDone", false)]
        private ICutscenePlayer nextCutscene;

        private void Awake()
        {
            player = GetComponentInChildren<VideoPlayer>();
            player.loopPointReached += (VideoPlayer playback) => Done();

            gameObject.SetActive(false);
        }

        public void Play()
        {
            gameObject.SetActive(true);
            player.Play();
        }

        private void Done()
        {
            if (transitionSceneWhenDone)
            {
                SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);
                return;
            }
            nextCutscene?.Play();
        }
    }
}
