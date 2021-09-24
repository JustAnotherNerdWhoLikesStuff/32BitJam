using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedBomb : IUseItem
{
    // Explostion memebers
    public float ExplosionForce_N = 10.0f;
    public float ExplosionRadius_m = 5.0f;
    public GameObject explosionParticle;

    // Timer and throw members
    public float TimeTilExplosion_s;
    public float ThrowSpeed_mpd;

    // internal timer/tracking members
    private bool timerStarted = false;
    private bool throwNextUpdate = false;
    private float startingTimeTilExplosion_s;

    // Fuse effect members
    private GameObject fuse;
    private GameObject fuseParticle;
    private float startingFuseYposition_m;
    private float fuseHeight_m;

    // Start is called before the first frame update
    void Start()
    {
        startingTimeTilExplosion_s = TimeTilExplosion_s;
        // Yeah, this is terrible, but it'll work for now.
        fuse = GameObject.Find("Fuse");
        startingFuseYposition_m = fuse.transform.localPosition.y;
        fuseHeight_m = fuse.transform.localScale.y * 2;
        ParticleSystem sparks = gameObject.GetComponentInChildren<ParticleSystem>();
        fuseParticle = sparks.gameObject;
        fuseParticle.SetActive(false);
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
            itemRigidBody.angularVelocity = new Vector3(4 * Mathf.PI * UnityEngine.Random.value,
                                                        4 * Mathf.PI * UnityEngine.Random.value,
                                                        4 * Mathf.PI * UnityEngine.Random.value);
            pickup.IsPickedUp = false;
            throwNextUpdate = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timerStarted)
        {
            TimeTilExplosion_s -= Time.deltaTime;

            // Adjust fuze position by how far in to timer we are
            fuse.transform.localPosition = new Vector3(
                fuse.transform.localPosition.x,
                startingFuseYposition_m - fuseHeight_m * (startingTimeTilExplosion_s - TimeTilExplosion_s) / startingTimeTilExplosion_s,
                fuse.transform.localPosition.z);

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

    private void Explode()
    {
        Instantiate(explosionParticle, transform.position, transform.rotation);
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

    // IUseItem overrides
    public override void OnPickup()
    {
        // No additional action
    }

    public override void OnPutdown()
    {
        // No additional action
    }

    public override void OnUse()
    {
        timerStarted = true;
        CanBePickedUp = false;
        fuseParticle.SetActive(true);
    }

    public override void OnAltUse()
    {
        throwNextUpdate = true;
    }
}
