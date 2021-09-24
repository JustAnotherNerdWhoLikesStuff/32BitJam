using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasdMovement : MonoBehaviour
{
    public float MovementSpeed_mpd;
    public float DashSpeed_mpd;
    public GameObject DashParticle;

    private Rigidbody playerRigidBody;
    private int dashNextNTicks;
    private Vector3 preDashVelocity_mpd;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (dashNextNTicks > 0)
        {
            playerRigidBody.velocity.Normalize();
            playerRigidBody.velocity *= DashSpeed_mpd;
            dashNextNTicks--;
        }
        else if (dashNextNTicks == 0)
        {
            playerRigidBody.velocity = preDashVelocity_mpd;
            // Messy but it works
            dashNextNTicks = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Use separate Ifs because the player can hold down more than one button.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(DashParticle, transform.position, transform.rotation);
            dashNextNTicks = 5;
            preDashVelocity_mpd = playerRigidBody.velocity;
        }
        else
        {
            Vector3 inputVector = new Vector3(0.0f, 0.0f, 0.0f);
            if (Input.GetKey(KeyCode.W))
                inputVector += new Vector3(0.0f, +1.0f);
            if (Input.GetKey(KeyCode.S))
                inputVector += new Vector3(0.0f, -1.0f);
            if (Input.GetKey(KeyCode.A))
                inputVector += new Vector3(+1.0f, 0.0f);
            if (Input.GetKey(KeyCode.D))
                inputVector += new Vector3(-1.0f, 0.0f);

            inputVector.Normalize();
            inputVector *= MovementSpeed_mpd;

            playerRigidBody.velocity = inputVector;
        }
    }
}
