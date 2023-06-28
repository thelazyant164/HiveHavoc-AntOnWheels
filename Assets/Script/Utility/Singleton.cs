using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static public T Instance
    {
        get { return _instance; }
        protected set { _instance = value; }
    }
    static private T _instance;
}
