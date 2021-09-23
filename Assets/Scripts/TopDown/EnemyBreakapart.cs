using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBreakapart : MonoBehaviour
{
    public enum KillForceType : int
    {
        Force,
        ForceAtPosition,
        ExplosionForce,
        Collision,
    }

    private EnemyHealth healthComponent;
    private new Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        // Verify
        if (!gameObject.TryGetComponent<EnemyHealth>(out healthComponent))
        {
            Debug.LogError("EnemyBreakapart gameObject missing EnemyHealth!  Add EnemyHealth component!");
        }
        if (!gameObject.TryGetComponent<Rigidbody>(out rigidbody))
        {
            Debug.LogError("EnemyBreakapart gameObject missing Rigidbody!  Add Rigidbody componenet!");
        }

        // Disable all children
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            // Exclude parent
            if (children[i].gameObject != gameObject)
                children[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Kill()
    {
        // Can't use regular loop here; GetComponentsInChildren ignores disabled gameObjects
        int numChildren = gameObject.transform.childCount;
        Transform[] children = new Transform[numChildren];
        for (int i = 0; i < numChildren; i++)
        {
            Transform child = gameObject.transform.GetChild(i);
            if (child.gameObject != gameObject)
            {
                // Add to second list so that we don't invalidate our iterators by deleting here.
                children[i] = child;
            }
        }
        for (int i = 0; i < numChildren; i++)
        {
            if (children[i] != null)
            {
                children[i].gameObject.SetActive(true);
                children[i].SetParent(null);

                Rigidbody childRigidBody = children[i].GetComponent<Rigidbody>();
                childRigidBody.mass = rigidbody.mass / numChildren;
                ForceMode forceMode = ForceMode.Impulse;

                // Apply most recent force to children
                switch (healthComponent.PreviousForceType)
                {
                    case KillForceType.Force:
                    {
                        // Might need to make the ForceMode another parameter later
                        childRigidBody.AddForce(
                            healthComponent.PreviousForce_N,
                            forceMode);
                        break;
                    }
                    case KillForceType.ForceAtPosition:
                    {
                        childRigidBody.AddForceAtPosition(
                            healthComponent.PreviousAtPointForce_N,
                            healthComponent.PreviousForceAtPointPosition_m,
                            forceMode);
                        break;
                    }
                    case KillForceType.ExplosionForce:
                    {
                        childRigidBody.AddExplosionForce(
                            healthComponent.PreviousExplosionForce_N,
                            healthComponent.PreviousExplosionPosition_m,
                            healthComponent.PreviousExplosionRadius_m,
                            0.0f, // upwards modifier
                            forceMode);
                        break;
                    }
                    case KillForceType.Collision:
                    {
                        childRigidBody.AddForceAtPosition(
                            healthComponent.PreviousCollisionImpulse_N.magnitude * healthComponent.PreviousCollisionPoint_m.normal,
                            healthComponent.PreviousCollisionPoint_m.point,
                            forceMode);
                        break;
                    }
                }
            }
        }

        Destroy(gameObject);
    }
}
