using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedBomb : IUseItem
{
    public float TimeTilExplosion_s;

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
            Vector3 angularVelocity = new Vector3(4 * Mathf.PI * UnityEngine.Random.value,
                                                  4 * Mathf.PI * UnityEngine.Random.value,
                                                  4 * Mathf.PI * UnityEngine.Random.value);
            gameObject.GetComponent<Throwable>().Throw(angularVelocity);

            Pickupable pickup = gameObject.GetComponent<Pickupable>();
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
                gameObject.GetComponent<Explosive>().Explode();
            }
        }
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
        fuseParticle.SetActive(true);
    }

    public override void OnAltUse()
    {
        throwNextUpdate = true;
    }
}
