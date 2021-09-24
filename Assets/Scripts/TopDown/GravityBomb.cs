using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBomb : IUseItem
{
    // Explosion members
    public float ExplosionForce_N;
    public float ExplosionRadius_m;
    public float PullForce_N;
    public GameObject ExplosionParticle;

    public float TimeTilExplosion_s;
    public float ThrowSpeed_mpd;

    private bool timerStarted = false;
    private bool throwNextUpdate = false;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // FixedUpdate is called once per physics simulation tick
    private void FixedUpdate()
    {
        if (throwNextUpdate)
        {
            Pickupable pickup = gameObject.GetComponent<Pickupable>();
            Rigidbody itemRigidBody = gameObject.GetComponent<Rigidbody>();
            itemRigidBody.isKinematic = false;
            Vector3 unityVector = new Vector3(0.0f, 0.0f, ThrowSpeed_mpd);
            Vector3 directionVector = pickup.Owner.transform.rotation * unityVector;

            itemRigidBody.velocity = directionVector;
            itemRigidBody.angularVelocity = new Vector3(0, 0, 0);
            pickup.IsPickedUp = false;
            throwNextUpdate = false;
        }
        if (timerStarted)
        {
            Charge();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (timerStarted)
        {
            TimeTilExplosion_s -= Time.deltaTime;

            if (TimeTilExplosion_s <= 0.0f + Mathf.Epsilon)
            {
                Explode();
            }
        }
    }

    private void OnDestroy()
    {
        foreach (Action callback in onDestroyDelegates)
        {
            callback();
        }
    }

    // This is copy pasta and I am not ashamed.
    private void Charge()
    {
        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, ExplosionRadius_m);

        foreach (Collider hit in colliders)
        {
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();

            if (rigidbody != null && hit.tag != "Item" && hit.tag != "Player")
                rigidbody.AddExplosionForce(-(PullForce_N), explosionPosition, ExplosionRadius_m, 0.0f, ForceMode.Impulse);
        }
    }

    private void Explode()
    {
        Instantiate(ExplosionParticle, transform.position, transform.rotation);
        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, ExplosionRadius_m);

        foreach (Collider hit in colliders)
        {
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();

            if (rigidbody != null && hit.tag != "Item" && hit.tag != "Player")
                rigidbody.AddExplosionForce(ExplosionForce_N, explosionPosition, ExplosionRadius_m, 0.0f);
        }

        Destroy(gameObject);
    }

    // IUseItem overloads
    public override void OnPickup()
    {
        // Some cool sweeping sounds should play here
    }

    public override void OnPutdown()
    {
        // No additional action
    }

    public override void OnUse()
    {
        timerStarted = true;
        CanBePickedUp = false;
    }

    public override void OnAltUse()
    {
        timerStarted = true;
        CanBePickedUp = false;
        throwNextUpdate = true;
    }
}
