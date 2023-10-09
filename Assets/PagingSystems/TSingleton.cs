using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSingleton<T> : BaseObject where T : BaseObject, new()
{
    private static T _instance = null;

    private static readonly object locker = new object();

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (locker)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }
}
