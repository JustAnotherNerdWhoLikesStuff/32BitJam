using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityInZ : MonoBehaviour
{
    public static float GravitationalAcceleration_mpd2 = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -GravitationalAcceleration_mpd2), ForceMode.Acceleration);
    }
}
