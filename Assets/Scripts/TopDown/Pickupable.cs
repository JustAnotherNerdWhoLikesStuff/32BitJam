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
            wasPickedUp = isPickedUp;
            isPickedUp = value;
        }
    }

    private bool isPickedUp = false;
    private bool wasPickedUp = false;
    private Collider owner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPickedUp)
        {
            transform.position = new Vector3(owner.transform.position.x, owner.transform.position.y + (owner.transform.lossyScale.y + transform.lossyScale.y / 2.0f), owner.transform.position.z);
        }
        if (IsPickedUp && !wasPickedUp)
        {
            Debug.Log("Picked up rising edge.");
            gameObject.GetComponent<Glow>().Glowing = false;
        }
        else if (wasPickedUp && !IsPickedUp)
        {
            Debug.Log("Picked up falling edge.");
            gameObject.GetComponent<Glow>().Glowing = true;
        }
        wasPickedUp = IsPickedUp;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entered collider.");
            owner = other;
            gameObject.GetComponent<Glow>().Glowing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Exited collider.");
            owner = null;
            gameObject.GetComponent<Glow>().Glowing = false;
        }
    }
}
