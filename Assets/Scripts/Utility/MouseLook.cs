using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.z - (gameObject.transform.lossyScale.z / 2.0f);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        gameObject.transform.LookAt(worldPosition, Vector3.forward);
    }
}
