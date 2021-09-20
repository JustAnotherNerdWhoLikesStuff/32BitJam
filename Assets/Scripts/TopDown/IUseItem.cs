using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUseItem : MonoBehaviour
{
    public bool CanBePickedUp = true;
    protected List<Action> onDestroyDelegates = new List<Action>();

    public void CollectDelegates(Action onDestroy)
    {
        onDestroyDelegates.Add(onDestroy);
    }

    public abstract void OnPickup();

    public abstract void OnPutdown();

    public abstract void OnUse();

    public abstract void OnAltUse();
}
