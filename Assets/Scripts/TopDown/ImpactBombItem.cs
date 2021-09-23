using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactBombItem : IUseItem
{
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
            Vector3 angularVelocity = new Vector3(4 * Mathf.PI * UnityEngine.Random.value,
                                                  4 * Mathf.PI * UnityEngine.Random.value,
                                                  4 * Mathf.PI * UnityEngine.Random.value);
            gameObject.GetComponent<Throwable>().Throw(angularVelocity);

            Pickupable pickup = gameObject.GetComponent<Pickupable>();
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
                gameObject.GetComponent<Explosive>().Explode();
            }
        }
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
        gameObject.GetComponent<Explosive>().Explode();
    }

    public override void OnAltUse()
    {
        localPickedUp = true;
        throwNextUpdate = true;
    }
}
