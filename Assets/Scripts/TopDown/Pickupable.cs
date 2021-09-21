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
                previousOrientation = gameObject.transform.rotation;
                gameObject.GetComponent<Glow>().Glowing = false;
                gameObject.GetComponent<SpinRelativeToXY>().Spin = false;
            }
            else
            {
                gameObject.transform.rotation = previousOrientation;
                gameObject.GetComponent<Glow>().Glowing = true;
                gameObject.GetComponent<SpinRelativeToXY>().Spin = true;
            }
        }
    }

    private bool isPickedUp = false;
    private Quaternion previousOrientation;
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
            gameObject.transform.rotation = owner.transform.rotation;
            float distanceFromCenter = owner.transform.lossyScale.y;
            // Don't know why this vector works, but it does.
            Vector3 objectPosition = new Vector3(0.0f, 0.0f, distanceFromCenter);
            Vector3 rotationPosition = owner.transform.rotation * objectPosition;
            gameObject.transform.position = owner.transform.position + rotationPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            owner = other;
            gameObject.GetComponent<Glow>().Glowing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            owner = null;
            gameObject.GetComponent<Glow>().Glowing = false;
        }
    }
}
