using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBottomMeshToZero : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.localScale.y, transform.position.z / 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"Current y position is {transform.position.y}");
    }
}
