using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinRelativeToXY : MonoBehaviour
{
    public float RotationRate_degpf;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.RotateAround(gameObject.transform.position, Vector3.back, RotationRate_degpf * Time.deltaTime);
    }
}
