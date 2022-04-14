using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    public static T Instance
    {
        get { return instance; }
    }

    public bool IsSingelton = false;
    public bool IsPersistant = false;

    public virtual void Awake()
    {
        if (IsSingelton)
        {
            if (!instance)
            {
                instance = this as T;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            instance = this as T;
        }

        if (IsPersistant)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public virtual void Init()
    {
    }

    protected virtual void OnDestroy()
    {
        //Debug.Log(typeof(T) + " is destroyed.");
        instance = null;
    }
}