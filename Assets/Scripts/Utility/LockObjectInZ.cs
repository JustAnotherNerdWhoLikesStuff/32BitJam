using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockObjectInZ : MonoBehaviour
{
    private float initialPositionZ_m;

    // Start is called before the first frame update
    void Start()
    {
        initialPositionZ_m = gameObject.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, initialPositionZ_m);
    }
}
