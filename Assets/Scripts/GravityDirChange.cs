using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityDirChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.I))
        {
            Physics.gravity = new Vector3(0, 9.8f, 0);
        }
        if (Input.GetKey(KeyCode.K))
        {
            Physics.gravity = new Vector3(0, -9.8f, 0);
        }
        if (Input.GetKey(KeyCode.J))
        {
            Physics.gravity = new Vector3(-9.8f, 0, 0);
        }
        if (Input.GetKey(KeyCode.L))
        {
            Physics.gravity = new Vector3(9.8f, 0, 0);
        }
    }
}
