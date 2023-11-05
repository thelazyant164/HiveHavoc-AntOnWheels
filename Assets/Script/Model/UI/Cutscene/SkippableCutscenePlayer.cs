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
    public sealed class SkippableCutscenePlayer : MonoBehaviour, ICutscenePlayer
    {
        [SerializeField]
        private string playbackPath,
            staticPath;

        private Image skipped;
        private VideoPlayer player;

        [SerializeField]
        private List<Sprite> statics;

        [SerializeField]
        private List<VideoClip> content;

        private static int CompareByNumericName<T>(T a, T b) where T : UnityEngine.Object
        {
            return Int32.Parse(a.name).CompareTo(Int32.Parse(b.name));
        }

        [SerializeField]
        private int playing = 0;

        [SerializeField]
        private bool transitionSceneWhenDone = false;

        [SerializeField, ShowWhen("transitionSceneWhenDone", true)]
        private string nextScene;

        // TODO: make generic interface
        [SerializeField, ShowWhen("transitionSceneWhenDone", false)]
        private CutscenePlayer nextCutscene;

        private event EventHandler OnSkip;

        private void Awake()
        {
            skipped = GetComponentInChildren<Image>();
            player = GetComponentInChildren<VideoPlayer>();

            OnSkip += (object sender, EventArgs e) =>
            {
                if (player.isPlaying)
                {
                    Skip();
                }
                else
                {
                    Next();
                }
            };

            player.started += (VideoPlayer playback) => skipped.sprite = statics[playing];

            player.loopPointReached += (VideoPlayer playback) => Next();

            Setup();
            Init();
            Play();
        }

        private void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
                OnSkip?.Invoke(this, EventArgs.Empty);
        }

        private void Setup()
        {
            content = Resources.LoadAll(playbackPath, typeof(VideoClip)).Cast<VideoClip>().ToList();
            statics = Resources.LoadAll(staticPath, typeof(Sprite)).Cast<Sprite>().ToList();

            content.Sort(CompareByNumericName);
            statics.Sort(CompareByNumericName);

            Assert.AreEqual(content.Count, statics.Count);
        }

        private void Init()
        {
            playing = 0;
            player.clip = content[playing];
            skipped.sprite = statics[playing];
        }

        private void Skip()
        {
            player.Pause();
            player.enabled = false;
        }

        private void Next()
        {
            if (playing >= content.Count - 1)
            {
                Done();
                return;
            }

            player.enabled = true;

            playing++;
            if (playing < content.Count)
            {
                player.clip = content[playing];
            }
            Play();
        }

        public void Play() => player.Play();

        private void Done()
        {
            if (transitionSceneWhenDone)
            {
                SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);
                return;
            }
            nextCutscene?.Play();
            gameObject.SetActive(false);
        }
    }
}
