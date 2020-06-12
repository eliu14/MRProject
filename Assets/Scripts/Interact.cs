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
    public bool usingDefaultGrav;
    // Update is called once per frame

    public AudioClip audio;
    AudioSource audio_source;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        cf = this.GetComponent<ConstantForce>();
        audio_source = GetComponent<AudioSource>();
    }
    void Update()
    {
        float distance = Vector3.Distance(this.transform.position, temp_parent.transform.position);

        if (usingDefaultGrav)
            rb.useGravity = true;
        else
            rb.useGravity = false;

        if (grab_action.GetState(right_hand) && distance < 0.5f)
        {
            isHeld = true;
            usingDefaultGrav = false;
            cf.force = Vector3.zero;
            rb.detectCollisions = false;
        }
        else
        {
            //check room gravity 
            if (!usingDefaultGrav)
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
            if (!usingDefaultGrav)
                checkGrav();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!usingDefaultGrav)
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

    void OnCollisionEnter(Collision collision)
    {
        if (audio != null)
        {
            audio_source.PlayOneShot(audio, 0.1f);
        }
    }
}
