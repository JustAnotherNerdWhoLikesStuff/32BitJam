using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    public float ThrowSpeed_mpd;

    public void Start()
    {
        Pickupable pickupable;
        if (!gameObject.TryGetComponent<Pickupable>(out pickupable))
        {
            Debug.LogError("A Throwable gameObject is not pickupable!  Add pickupable component!");
        }
        Rigidbody rigidbody;
        if (!gameObject.TryGetComponent<Rigidbody>(out rigidbody))
        {
            Debug.LogError("A Throwable gameObject does not have a Rigidbody!  Add Rigidbody component!");
        }
    }

    public void Throw(Vector3 angularVelocity)
    {
        Pickupable pickup = gameObject.GetComponent<Pickupable>();
        Rigidbody itemRigidBody = gameObject.GetComponent<Rigidbody>();
        itemRigidBody.isKinematic = false;
        Vector3 unityVector = new Vector3(0.0f, 0.0f, ThrowSpeed_mpd);
        Vector3 directionVector = pickup.Owner.transform.rotation * unityVector;

        itemRigidBody.velocity = directionVector;
        itemRigidBody.angularVelocity = angularVelocity;
    }
}
