using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public bool IsPickedUp
    {
        get { return isPickedUp; }
        set
        {
            isPickedUp = value;
            if (isPickedUp)
            {
                initialOrientation = gameObject.transform.rotation;
                IUseItem itemInterfaceComponent;
                if (gameObject.TryGetComponent<IUseItem>(out itemInterfaceComponent))
                    itemInterfaceComponent.OnPickup();
                Glow glowComponent;
                if (gameObject.TryGetComponent<Glow>(out glowComponent))
                    glowComponent.Glowing = false;
                SpinRelativeToXY spinComponent;
                if (gameObject.TryGetComponent<SpinRelativeToXY>(out spinComponent))
                    spinComponent.Spin = false;
            }
            else
            {
                gameObject.transform.rotation = initialOrientation;
                IUseItem itemInterfaceComponent;
                if (gameObject.TryGetComponent<IUseItem>(out itemInterfaceComponent))
                {
                    itemInterfaceComponent.OnPutdown();
                }
                if (itemInterfaceComponent.CanBePickedUp)
                {
                    Glow glowComponent;
                    if (gameObject.TryGetComponent<Glow>(out glowComponent))
                        glowComponent.Glowing = true;
                    SpinRelativeToXY spinComponent;
                    if (gameObject.TryGetComponent<SpinRelativeToXY>(out spinComponent))
                        spinComponent.Spin = true;
                }
            }
        }
    }

    public Collider Owner { get; private set; }

    private bool isPickedUp = false;
    private Quaternion initialOrientation;

    // Start is called before the first frame update
    private void Start()
    {
        initialOrientation = gameObject.transform.rotation;
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsPickedUp)
        {
            if (Owner)
            {
                gameObject.transform.rotation = Owner.transform.rotation;
                float distanceFromCenter = Owner.transform.lossyScale.y;
                Vector3 objectPosition = new Vector3(0.0f, 0.0f, distanceFromCenter);
                Vector3 rotationPosition = Owner.transform.rotation * objectPosition;
                gameObject.transform.position = Owner.transform.position + rotationPosition;
            }
        }
    }

    private void OnDestroy()
    {
        Owner = null;
    }

    public void UseItem()
    {
        foreach (IUseItem item in gameObject.GetComponents<IUseItem>())
        {
            item.OnUse();
        }
    }

    public void UseItemAlternate()
    {
        foreach (IUseItem item in gameObject.GetComponents<IUseItem>())
        {
            item.OnAltUse();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IUseItem itemInterfaceComponent;
            if (gameObject.TryGetComponent<IUseItem>(out itemInterfaceComponent))
            {
                if (itemInterfaceComponent.CanBePickedUp)
                {
                    Owner = other;
                    gameObject.GetComponent<Glow>().Glowing = itemInterfaceComponent.CanBePickedUp;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Owner = null;
            gameObject.GetComponent<Glow>().Glowing = false;
        }
    }
}
