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
            item.GetComponent<Pickupable>().IsPickedUp = !item.GetComponent<Pickupable>().IsPickedUp;
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
