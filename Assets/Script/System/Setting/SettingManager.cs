using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Setting
{
    public enum Setting
    {
        MouseAimSensitivity,
        KeyboardAimSensitivity,
        ConsoleAimSensitivity,
        ConsoleSpamSensitivity
    }

    [Serializable]
    public sealed class SettingData : ISettingBar
    {
        [SerializeField]
        internal Setting setting;

        [SerializeField]
        private float value;
        public float Value
        {
            get => value;
            set
            {
                this.value = value;
                OnChange?.Invoke(this, value);
            }
        }

        [SerializeField]
        private float minValue;
        public float MinValue
        {
            get => minValue;
        }

        [SerializeField]
        private float maxValue;
        public float MaxValue
        {
            get => maxValue;
        }

        internal event EventHandler<float> OnChange;
    }

    public sealed class SettingManager : Singleton<SettingManager>
    {
        [SerializeField]
        private List<SettingData> settings;
        internal SettingData this[Setting setting] =>
            settings.Find(settingConfig => settingConfig.setting == setting);
        internal bool IsOverriden => settings?.Count > 0;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);

            ClampToDefault();
        }

        private void ClampToDefault() // TODO: write persistant setting data to disk, then read & set here
        {
            foreach (SettingData setting in settings) { }
        }
    }
}
