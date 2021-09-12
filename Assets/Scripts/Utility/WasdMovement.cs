using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasdMovement : MonoBehaviour
{
    public float MovementSpeed_mpf;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Use separate Ifs because the player can hold down more than one button.
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + MovementSpeed_mpf * Time.deltaTime, transform.position.z);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - MovementSpeed_mpf * Time.deltaTime, transform.position.z);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x + MovementSpeed_mpf * Time.deltaTime, transform.position.y, transform.position.z);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x - MovementSpeed_mpf * Time.deltaTime, transform.position.y, transform.position.z);
        }
    }
}
