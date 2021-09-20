using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasdMovement : MonoBehaviour
{
    public float MovementSpeed_mpf;
    public GameObject dashParticle;
    public float dashSpeed;
    public float startDashTime;

    public float explosionForce = 10.0f;
    public float explosionRadius = 5.0f;
    public GameObject explosionParticle;

    private Rigidbody playerRigidBody;
    private int dashNextNTicks;
    private Vector3 preDashVelocity_mpd;

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Use separate Ifs because the player can hold down more than one button.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(dashParticle, transform.position, Quaternion.identity);
            if (Input.GetKey(KeyCode.W))
                transform.position = new Vector3(transform.position.x, transform.position.y + dashSpeed, transform.position.z);
            if (Input.GetKey(KeyCode.S))
                transform.position = new Vector3(transform.position.x, transform.position.y - dashSpeed, transform.position.z);
            if (Input.GetKey(KeyCode.A))
                transform.position = new Vector3(transform.position.x + dashSpeed, transform.position.y, transform.position.z);
            if (Input.GetKey(KeyCode.D))
                transform.position = new Vector3(transform.position.x - dashSpeed, transform.position.y, transform.position.z);
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
                transform.position = new Vector3(transform.position.x, transform.position.y + MovementSpeed_mpf * Time.deltaTime, transform.position.z);
            if (Input.GetKey(KeyCode.S))
                transform.position = new Vector3(transform.position.x, transform.position.y - MovementSpeed_mpf * Time.deltaTime, transform.position.z);
            if (Input.GetKey(KeyCode.A))
                transform.position = new Vector3(transform.position.x + MovementSpeed_mpf * Time.deltaTime, transform.position.y, transform.position.z);
            if (Input.GetKey(KeyCode.D))
                transform.position = new Vector3(transform.position.x - MovementSpeed_mpf * Time.deltaTime, transform.position.y, transform.position.z);
        }
    }
}
