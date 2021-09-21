using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownsampleController : MonoBehaviour
{
    public KeyCode EnableKey = KeyCode.Tab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(EnableKey))
        {
            gameObject.GetComponent<DownsampleCamera>().enabled = !gameObject.GetComponent<DownsampleCamera>().enabled;
        }
    }
}
