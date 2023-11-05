using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Slider))]
public abstract class ProgressBar : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void Bind(IProgressBar progress)
    {
        if (slider == null)
            return;
        slider.maxValue = progress.MaxValue;
        slider.value = progress.Value;
        progress.OnValueChange += (object sender, float value) => slider.value = value;
    }

    public void Show()
    {
        slider.gameObject.SetActive(true);
    }

    public void Hide()
    {
        slider.gameObject.SetActive(false);
    }
}
