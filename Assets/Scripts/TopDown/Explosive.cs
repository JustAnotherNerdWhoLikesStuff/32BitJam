using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public float ExplosionForce_N = 10.0f;
    public float ExplosionRadius_m = 5.0f;
    public GameObject ExplosionParticle;

    public void Explode()
    {
        Instantiate(ExplosionParticle, transform.position, transform.rotation);
        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, ExplosionRadius_m);

        foreach (Collider hit in colliders)
        {
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();

            if (rigidbody != null && hit.tag != "Item" && hit.tag != "Player")
            {
                rigidbody.AddExplosionForce(ExplosionForce_N, explosionPosition, ExplosionRadius_m, 0.0f);

                if (hit.tag == "Enemy")
                {
                    hit.gameObject.GetComponent<EnemyHealth>().ApplyExplosionDamage(ExplosionForce_N, explosionPosition, ExplosionRadius_m);
                }
            }
        }

        Destroy(gameObject);
    }
}
