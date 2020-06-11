using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Interact : MonoBehaviour
{
    bool isHeld = false;
    public GameObject temp_parent;
    public GameObject player;
    public float force_mult = 1500.0f;

    public SteamVR_Input_Sources right_hand;
    public SteamVR_Action_Boolean grab_action;

    public Rigidbody rb;
    ConstantForce cf;
    // 0 - down or -Y
    // 1 - left or -X
    // 2 - Up or Y
    public int gravity_flag = 0;

    // Update is called once per frame

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        cf = this.GetComponent<ConstantForce>();
        rb.useGravity = false;
    }
    void Update()
    {
        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        if (grab_action.GetState(right_hand) && distance < 3.0f)
        {
            isHeld = true;
           //rb.useGravity = false;
            cf.force = Vector3.zero;
           rb.detectCollisions = false;
        }
        else
        {
            //check room gravity 
            //rb.useGravity = true;
            checkGrav();
            rb.detectCollisions = true;
            isHeld = false;
        }
        if (isHeld)
        {
           rb.velocity = Vector3.zero;
           rb.angularVelocity = Vector3.zero;
            this.transform.SetParent(temp_parent.transform);
            /*if (Input.GetMouseButtonDown(1))
            {
               rb.AddForce(temp_parent.transform.forward * force_mult);
                isHeld = false;
            }*/
        }
        else
        {
            this.transform.SetParent(null);
            //rb.useGravity = true;
            checkGrav();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GravDown"))
        {
            gravity_flag = 0;
            checkGrav();
        }
        else if (other.gameObject.CompareTag("GravSide"))
        {
            gravity_flag = 1;
            checkGrav();
        }
        else if (other.gameObject.CompareTag("GravUp"))
        {
            gravity_flag = 2;
            checkGrav();
        }
    }

    void checkGrav()
    {
        if (gravity_flag == 0)
            cf.force = new Vector3(0, -9.8f, 0);
        if (gravity_flag == 1)
            cf.force = new Vector3(-9.8f, 0, 0);
        if (gravity_flag == 2)
            cf.force = new Vector3(0, 9.8f, 0);
    }
    void OnMouseDown()
    {
        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        if (distance > 1.5f)
        {
            return;
        }
        isHeld = true;
       rb.useGravity = false;
       rb.detectCollisions = true;
    }

    void OnMouseUp()
    {      
       rb.useGravity = true;
       rb.detectCollisions = true;
        isHeld = false;
    }
}
