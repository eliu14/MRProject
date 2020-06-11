using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    bool isHold = false;
    public GameObject temporaryParent;
    public float forceMulti = 800.0f;

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(this.transform.position, temporaryParent.transform.position);
        if (distance < 5.0f)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isHold)
                {
                    this.GetComponent<Rigidbody>().useGravity = false;
                    this.GetComponent<Rigidbody>().detectCollisions = true;
                    isHold = true;
                }
                else
                {
                    this.GetComponent<Rigidbody>().useGravity = true;
                    this.GetComponent<Rigidbody>().detectCollisions = true;
                    isHold = false;

                }
            }
        }
        if (isHold)
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            this.transform.SetParent(temporaryParent.transform);
            if(Input.GetKeyDown(KeyCode.R))
            {
                this.GetComponent<Rigidbody>().AddForce(temporaryParent.transform.forward * forceMulti);
                isHold = false;
            }
        }
        else
        {
            this.transform.SetParent(null);
            this.GetComponent<Rigidbody>().useGravity = true;
            
        }
    }
}
