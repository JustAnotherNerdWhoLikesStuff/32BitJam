using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBomb : IUseItem
{
    // Explosion members
    public float PullRadius_m;
    public float PullForce_N;
    public GameObject ExplosionParticle;

    // Timer members
    public float TimeTilExplosion_s;
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
            Vector3 angularVelocity = Vector3.zero;
            gameObject.GetComponent<Throwable>().Throw(angularVelocity);

            Pickupable pickup = gameObject.GetComponent<Pickupable>();
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
                gameObject.GetComponent<Explosive>().Explode();
            }
        }
    }

    // This is copy pasta and I am not ashamed.
    private void Charge()
    {
        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, PullRadius_m);

        foreach (Collider hit in colliders)
        {
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();

            if (rigidbody != null && hit.tag != "Item" && hit.tag != "Player")
                rigidbody.AddExplosionForce(-(PullForce_N), explosionPosition, PullRadius_m, 0.0f, ForceMode.Impulse);
        }
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
    }

    public override void OnAltUse()
    {
        timerStarted = true;
        CanBePickedUp = false;
        throwNextUpdate = true;
    }
}
