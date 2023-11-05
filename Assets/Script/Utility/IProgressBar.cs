using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IProgressBar
{
    public float Value { get; }
    public float MaxValue { get; }
    public event EventHandler<float> OnValueChange;
}
