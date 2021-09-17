using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float detectionRadius = 466f;
    public float speed = 3f;
    public GameObject player;

    //public Collider detectionCollider = new Collider()

    private Transform target = null;
    private float stopDistance = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        var detectionCollider = gameObject.GetComponent<SphereCollider>();
        detectionCollider.radius = detectionRadius;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Debug.Log("LookAt");
            //transform.LookAt(target);
            transform.LookAt(target, Vector3.forward);
            var distance = Vector3.Distance(transform.position, target.position);
            Debug.Log($"distance = {distance}");

            if (distance < detectionRadius && distance > stopDistance)
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            target = other.transform;
            Debug.Log("Set target");
        }
        Debug.Log("TriggerEnter");
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            target = null;
            Debug.Log("Set target to null");
        }
        Debug.Log("TriggerExit");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(GetComponent<Renderer>().bounds.center, detectionRadius);
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
