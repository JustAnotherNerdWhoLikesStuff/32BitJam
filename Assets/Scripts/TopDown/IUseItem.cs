using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUseItem : MonoBehaviour
{
    public bool CanBePickedUp = true;

    public abstract void OnPickup();

    public abstract void OnPutdown();

    public abstract void OnUse();

    public abstract void OnAltUse();
}
