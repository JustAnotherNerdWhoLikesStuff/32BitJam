using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityInZ : MonoBehaviour
{
    public float GravitationalAccel_mpd2 = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Rigidbody>().AddForceAtPosition(new Vector3(0, 0, -GravitationalAccel_mpd2), gameObject.transform.position, ForceMode.Acceleration);
    }
}
