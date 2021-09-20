using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupper : MonoBehaviour
{
    private Collider item;
    private bool canPickup = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canPickup && Input.GetKeyDown(KeyCode.E))
        {
            Pickupable pickup;
            bool hasPickupable = item.TryGetComponent<Pickupable>(out pickup);
            if (hasPickupable)
            {
                if (canPickup && Input.GetKeyDown(KeyCode.E))
                {
                    pickup.IsPickedUp = !pickup.IsPickedUp;
                }
                if (pickup.IsPickedUp)
                {
                    if (Input.GetMouseButtonDown(leftClickCode))
                    {
                        pickup.UseItem();
                    }
                    if (Input.GetMouseButtonDown(rightClickCode))
                    {
                        pickup.UseItemAlternate();
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            canPickup = true;
            item = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            canPickup = false;
            item = null;
        }
    }
}
