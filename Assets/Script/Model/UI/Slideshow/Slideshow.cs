using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.UI
{
    public sealed class Slideshow : MonoBehaviour
    {
        [SerializeField]
        private Image slideshow;

        [SerializeField]
        private Button back;

        [SerializeField]
        private Button next;

        [SerializeField]
        private List<Sprite> slides;

        [SerializeField]
        private int currentSlide;

        private void Awake()
        {
            Assert.AreNotEqual(slides.Count, 0);

            back.onClick.AddListener(Back);
            next.onClick.AddListener(Next);

            back.interactable = false;
            next.interactable = slides.Count > 1;
        }

        private void Back()
        {
            currentSlide--;
            next.interactable = true;

            slideshow.sprite = slides[currentSlide];
            if (currentSlide == 0)
            {
                back.interactable = false;
            }
        }

        private void Next()
        {
            currentSlide++;
            back.interactable = true;

            slideshow.sprite = slides[currentSlide];
            if (currentSlide == slides.Count - 1)
            {
                next.interactable = false;
            }
        }
    }
}
