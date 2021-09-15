using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCamera : MonoBehaviour
{
    public GameObject objectToFollow;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Follow object's position but not orientation
        gameObject.transform.position = new Vector3(objectToFollow.transform.position.x, objectToFollow.transform.position.y, gameObject.transform.position.z);
    }
}
