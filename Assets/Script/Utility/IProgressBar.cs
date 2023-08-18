using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IProgressBar
{
    public abstract event EventHandler<float> OnValueChange;

    public abstract float MaxValue { get; }
    public abstract float Value { get; }
}