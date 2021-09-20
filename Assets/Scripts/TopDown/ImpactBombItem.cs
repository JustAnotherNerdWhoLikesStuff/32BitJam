using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactBombItem : MonoBehaviour, IUseItem
{
    public float explosionForce = 10.0f;
    public float explosionRadius = 5.0f;
    public GameObject explosionParticle;

    public float ThrowMultiplier;
    private bool throwNextUpdate = false;

    // FixedUpdate is called once per physics tick
    void FixedUpdate()
    {
        if (throwNextUpdate)
        {
            Pickupable pickup = gameObject.GetComponent<Pickupable>();

            Rigidbody itemRigidBody = gameObject.GetComponent<Rigidbody>();
            Vector3 unityVector = new Vector3(0.0f, 0.0f, ThrowMultiplier * (1.0f / itemRigidBody.mass));
            Vector3 directionVector = pickup.Owner.transform.rotation * unityVector;

            gameObject.GetComponent<Rigidbody>().AddForce(directionVector, ForceMode.Impulse);

            pickup.IsPickedUp = false;
            throwNextUpdate = false;
        }
    }

    private void Explode()
    {
        Instantiate(explosionParticle, transform.position, transform.rotation);
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null && hit.tag != "Item")
                rb.AddExplosionForce(explosionForce, explosionPos, explosionRadius, 0.0f);
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player"))
        {
            Explode();
        }
    }

    // IUseItem Implementation
    public void Use() 
    {
        // Just blow up, lol
        Explode();
    }

    public void AltUse()
    {
        throwNextUpdate = true;
    }
}
