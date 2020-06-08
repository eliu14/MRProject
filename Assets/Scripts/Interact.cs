using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Interact : MonoBehaviour
{
    bool isHeld = false;
    public GameObject temp_parent;
    public GameObject item;
    public GameObject player;
    public float force_mult = 1500.0f;

    public SteamVR_Input_Sources right_hand;
    public SteamVR_Action_Boolean grab_action;
    // Update is called once per frame
    void Update()
    {
        
        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        if (grab_action.GetState(right_hand) && distance < 3.0f)
        {
            isHeld = true;
            this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<Rigidbody>().detectCollisions = true;
        }
        else
        {
            this.GetComponent<Rigidbody>().useGravity = true;
            this.GetComponent<Rigidbody>().detectCollisions = true;
            isHeld = false;
        }
        if (isHeld)
        {
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            this.transform.SetParent(temp_parent.transform);
            /*if (Input.GetMouseButtonDown(1))
            {
                this.GetComponent<Rigidbody>().AddForce(temp_parent.transform.forward * force_mult);
                isHeld = false;
            }*/
        }
        else
        {
            this.transform.SetParent(null);
            this.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    void OnMouseDown()
    {
        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        if (distance > 1.5f)
        {
            return;
        }
        isHeld = true;
        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<Rigidbody>().detectCollisions = true;
    }

    void OnMouseUp()
    {      
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().detectCollisions = true;
        isHeld = false;
    }
}
