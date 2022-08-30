using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class UnlockableField: MonoBehaviour
{
    public UnityEvent OnUnlocked;
    public UnityEvent OnInitunlocked;
    private bool _isActivated;
    public abstract void Unlock();
    public abstract void InitUnlock();

    public Vector3 GetWorldPos()
    {
        return transform.position;
    }

    public bool IsActivated()
    {
        return _isActivated;
    }
    
    
}
