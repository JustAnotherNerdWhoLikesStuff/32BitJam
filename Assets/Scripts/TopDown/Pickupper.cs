using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupper : MonoBehaviour
{
    private List<Collider> items = new List<Collider>();
    private Pickupable pickedUpItem;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Only process one of these if they all happen on the same frame
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E");
            if (pickedUpItem &&
                pickedUpItem.IsPickedUp)
            {
                Debug.Log("IsHeld");
                // Drop current item
                pickedUpItem.IsPickedUp = false;
                pickedUpItem = null;
            }
            else
            {
                Debug.Log("IsNotHeld");
                // Remove null items from previous deletions
                List<Collider> itemsToRemove = new List<Collider>();
                foreach (Collider item in items)
                {
                    if (!item)
                    {
                        itemsToRemove.Add(item);
                    }
                }
                foreach (Collider item in itemsToRemove)
                {
                    Debug.Log("Hey, something was removed.");
                    items.Remove(item);
                }

                // Check if there are items to pick up.
                if (items.Count > 0)
                {
                    Debug.Log("Item(s) Nearby");
                    // Find closest item
                    Collider closestItem = items[0];
                    // Only do the loop if there's more than one to choose from.
                    if (items.Count > 1)
                    {
                        Debug.Log("More than one.");
                        float closestDistance = float.MaxValue;
                        for (int i = 0; i < items.Count; i++)
                        {
                            float distance = Vector3.Distance(gameObject.transform.position, items[i].transform.position);
                            if (distance < closestDistance)
                            {
                                closestDistance = distance;
                                closestItem = items[i];
                            }
                        }

                    }

                    Debug.Log("Item selected.");
                    pickedUpItem = closestItem.GetComponent<Pickupable>();
                    pickedUpItem.IsPickedUp = true;
                }
            }
        }
        else if (Input.GetMouseButtonDown(leftClickCode))
        {
            if (pickedUpItem)
            {
                pickedUpItem.UseItem();
            }
        }
        else if (Input.GetMouseButtonDown(rightClickCode))
        {
            if (pickedUpItem)
            {
                pickedUpItem.UseItemAlternate();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            items.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            items.Remove(other);
        }
    }
}
