using Com.StillFiveAsianStudios.HiveHavocAntOnWheels.Setting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISettingBar
{
    public float Value { get; set; }
    public float MinValue { get; }
    public float MaxValue { get; }
}
