using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactBombItem : IUseItem
{
    public float explosionForce_N = 10.0f;
    public float explosionRadius_m = 5.0f;
    public GameObject explosionParticle;

    public float ThrowSpeed_mpd;
    private bool throwNextUpdate = false;
    private bool localPickedUp = false;

    public float LightMinMagnitude;
    public float LightMaxMagnitude;
    public float LightPeriod;

    private void Start()
    {
        Light[] lights = gameObject.GetComponentsInChildren<Light>();
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].enabled = false;
        }
    }

    // FixedUpdate is called once per physics tick
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
            itemRigidBody.angularVelocity = new Vector3(4 * Mathf.PI * UnityEngine.Random.value,
                                                        4 * Mathf.PI * UnityEngine.Random.value,
                                                        4 * Mathf.PI * UnityEngine.Random.value);
            pickup.IsPickedUp = false;
            throwNextUpdate = false;
            CanBePickedUp = false;
            LightPeriod = 10;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // Use the local variable here in addition to Pickupable.IsPickedUp
        // so that it keeps glowing after being thrown.
        if (gameObject.GetComponent<Pickupable>().IsPickedUp || localPickedUp)
        {
            float amplitude = LightMaxMagnitude - LightMinMagnitude;
            float currentIntensity = amplitude * Mathf.Cos(Mathf.PI * LightPeriod * Time.time) + LightMinMagnitude;

            Light[] lights = gameObject.GetComponentsInChildren<Light>();
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].intensity = currentIntensity;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player") && !collision.collider.CompareTag("Item"))
        {
            Pickupable pickup = gameObject.GetComponent<Pickupable>();
            if (pickup.IsPickedUp || localPickedUp)
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

    private void Explode()
    {
        Instantiate(explosionParticle, transform.position, transform.rotation);
        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius_m);

        foreach (Collider hit in colliders)
        {
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();

            if (rigidbody != null && hit.tag != "Item")
                rigidbody.AddExplosionForce(explosionForce_N, explosionPosition, explosionRadius_m, 0.0f);
        }

        Destroy(gameObject);
    }

    // IUseItem Implementation
    public override void OnPickup()
    {
        Light[] lights = gameObject.GetComponentsInChildren<Light>();
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].enabled = true;
        }
    }

    public override void OnPutdown()
    {
        // Use the local variable here so that it keeps glowing after being thrown.
        if (!localPickedUp)
        {
            Light[] lights = gameObject.GetComponentsInChildren<Light>();
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].enabled = false;
            }
        }
    }

    public override void OnUse()
    {
        // Just blow up, lol
        Explode();
    }

    public override void OnAltUse()
    {
        localPickedUp = true;
        throwNextUpdate = true;
    }
}
