using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IService<T> where T : IService<T>
{
    public void Register(IServiceProvider<T> provider);
}
