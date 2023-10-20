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
    public sealed class CutscenePlayer : MonoBehaviour
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
        private string nextScene;

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
            player.Play();
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
                SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Single);
                return;
            }

            player.enabled = true;

            playing++;
            if (playing < content.Count)
            {
                player.clip = content[playing];
            }
            player.Play();
        }
    }
}
