using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Setting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public sealed class SettingBar : MonoBehaviour
{
    private Slider slider;

    [SerializeField]
    private Setting setting;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        Bind(SettingManager.Instance[setting]);
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveAllListeners();
    }

    public void Bind(ISettingBar setting)
    {
        if (slider == null)
            return;
        slider.minValue = setting.MinValue;
        slider.maxValue = setting.MaxValue;
        slider.value = setting.Value;
        slider.onValueChanged.AddListener(value => setting.Value = value);
    }
}
