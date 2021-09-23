using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float Health_N = 0;
    public float StaringHealth_N { get; private set; } = 0;

    // General force members
    public EnemyBreakapart.KillForceType PreviousForceType { get; private set; } = EnemyBreakapart.KillForceType.Collision;

    // General force members
    public Vector3 PreviousForce_N { get; private set; } = Vector3.zero;

    // ForceAtPoint members
    public Vector3 PreviousAtPointForce_N { get; private set; } = Vector3.zero;
    public Vector3 PreviousForceAtPointPosition_m { get; private set; } = Vector3.zero;

    // Explosion members
    public float PreviousExplosionForce_N { get; private set; } = 0.0f;
    public float PreviousExplosionRadius_m { get; private set; } = 0.0f;
    public Vector3 PreviousExplosionPosition_m { get; private set; } = Vector3.zero;

    // Collision members
    public Vector3 PreviousCollisionImpulse_N { get; private set; } = Vector3.zero;
    public ContactPoint PreviousCollisionPoint_m { get; private set; } = new ContactPoint();

    private new Rigidbody rigidbody;
    private EnemyBreakapart enemyBreakapart;

    // Start is called before the first frame update
    void Start()
    {
        // Checks
        if (!gameObject.TryGetComponent<Rigidbody>(out rigidbody))
        {
            Debug.LogError("EnemyHealth gameObject is missing Rigidbody!  Add Rigidbody component!");
        }
        if (!gameObject.TryGetComponent<EnemyBreakapart>(out enemyBreakapart))
        {
            Debug.LogError("EnemyHealth gameObject is missing EnemyBreakapart!  Add EnemyBreakapart component!");
        }

        rigidbody = gameObject.GetComponent<Rigidbody>();
        StaringHealth_N = Health_N;
    }

    // Update is called once per frame
    void Update()
    {
        if (Health_N <= 0 + float.Epsilon)
        {
            enemyBreakapart.Kill();
        }
    }

    public void ApplyForceDamage(Vector3 force_N, EnemyBreakapart.KillForceType type)
    {
        PreviousForce_N = force_N;
        PreviousForceType = EnemyBreakapart.KillForceType.Force;

        Health_N -= PreviousForce_N.magnitude;
        Debug.Log($"Current health: {Health_N}");
    }

    public void ApplyForceAtPointDamage(Vector3 force_N, Vector3 forcePoint_m, EnemyBreakapart.KillForceType type)
    {
        PreviousAtPointForce_N = force_N;
        PreviousForceAtPointPosition_m = forcePoint_m;
        PreviousForceType = EnemyBreakapart.KillForceType.ForceAtPosition;

        Health_N -= PreviousAtPointForce_N.magnitude;
        Debug.Log($"Current health: {Health_N}");
    }

    public void ApplyExplosionDamage(float force_N, Vector3 explosionPoint_m, float explosionRadius_m)
    {
        PreviousExplosionForce_N = force_N;
        PreviousExplosionPosition_m = explosionPoint_m;
        PreviousForceType = EnemyBreakapart.KillForceType.ExplosionForce;

        Health_N -= PreviousExplosionForce_N;
        Debug.Log($"Current health: {Health_N}");
    }

    public void ApplyCollisionDamage(Vector3 force_N, ContactPoint collisionPoint_m)
    {
        PreviousCollisionImpulse_N = force_N;
        PreviousCollisionPoint_m = collisionPoint_m;
        PreviousForceType = EnemyBreakapart.KillForceType.Collision;

        Health_N -= PreviousCollisionImpulse_N.magnitude;
        Debug.Log($"Current health: {Health_N}");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Floor"))
        {
            Rigidbody colliderBody = collision.rigidbody;

            if (colliderBody != null)
            {
                // TODO: figure out the best way to do multiple contacts
                ApplyCollisionDamage(collision.impulse, collision.GetContact(0));
            }
        }
    }
}
