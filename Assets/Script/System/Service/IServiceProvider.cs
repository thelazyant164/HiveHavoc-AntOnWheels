using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IServiceProvider<T> where T : IService<T>
{
    public event EventHandler<T> OnAvailable;

    public void Register(T service);
}
