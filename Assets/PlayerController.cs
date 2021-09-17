using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControls playerControls;
    public float MovementSpeed_mpf;
    public GameObject dashParticle;
    public float dashSpeed;
    public float startDashTime;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // read the movement value
        Vector2 movementInput = playerControls.Player.Move.ReadValue<Vector2>();
        Vector2 rotation = playerControls.Player.Look.ReadValue<Vector2>();

        Debug.Log(rotation.x);

        transform.position = new Vector3(transform.position.x + ((MovementSpeed_mpf * (-movementInput.x)) * Time.deltaTime), transform.position.y + ((MovementSpeed_mpf * (movementInput.y)) * Time.deltaTime), transform.position.z);
        transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, rotation.x, transform.rotation.w);
        // Move the player
    }
}
