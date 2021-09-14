using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMovement : MonoBehaviour
{
    public float MovementSpeed_mpf;
    public GameObject dashParticle;
    public float dashSpeed;
    public float startDashTime;

    private Rigidbody rbPlayer;

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Use separate Ifs because the player can hold down more than one button.
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            // do nothing. keyboard is turned off

            // some input *still* gets through, fix this later. JUST USE THE DANG CONTROLLER IF YOU'RE GOING TO HAVE THIS ENABLED
        }
        else
        {
            if (Input.GetButtonDown("Fire3"))   // PS - Circle
            {
                Instantiate(dashParticle, transform.position, Quaternion.identity);
                if (verticalInput > 0)
                    transform.position = new Vector3(transform.position.x, transform.position.y + dashSpeed, transform.position.z);
                if (verticalInput < 0)
                    transform.position = new Vector3(transform.position.x, transform.position.y - dashSpeed, transform.position.z);
                if (horizontalInput < 0)
                    transform.position = new Vector3(transform.position.x + dashSpeed, transform.position.y, transform.position.z);
                if (horizontalInput > 0)
                    transform.position = new Vector3(transform.position.x - dashSpeed, transform.position.y, transform.position.z);
            }
            else
            {
                if (verticalInput > 0)
                    transform.position = new Vector3(transform.position.x, transform.position.y + MovementSpeed_mpf * Time.deltaTime, transform.position.z);
                if (verticalInput < 0)
                    transform.position = new Vector3(transform.position.x, transform.position.y - MovementSpeed_mpf * Time.deltaTime, transform.position.z);
                if (horizontalInput < 0)
                    transform.position = new Vector3(transform.position.x + MovementSpeed_mpf * Time.deltaTime, transform.position.y, transform.position.z);
                if (horizontalInput > 0)
                    transform.position = new Vector3(transform.position.x - MovementSpeed_mpf * Time.deltaTime, transform.position.y, transform.position.z);
            }
        }
    }
}
