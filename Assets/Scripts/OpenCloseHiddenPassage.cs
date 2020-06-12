using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class OpenCloseHiddenPassage : MonoBehaviour
{
    public GameObject reactable;
    public GameObject player;
    bool isOpen = false;
    bool isMoving = false;
    Vector3 current_loc;
    Vector3 open_vector;
    Vector3 close_vector;

    public SteamVR_Input_Sources right_hand;
    public SteamVR_Action_Boolean activate_passage_action;

    public AudioClip audio;
    AudioSource audio_source;

    // Start is called before the first frame update
    void Start()
    {
        current_loc = transform.localPosition;
        open_vector = new Vector3(-0.01f, 0.0f, 0.0f);
        close_vector = new Vector3(0.01f, 0.0f, 0.0f);
        audio_source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("Update isOpen: " + isOpen);
        //Debug.Log("Update isMoving: " + isMoving);
        //Debug.Log("Update Current_Loc_x: " + current_loc.x);
        if (!isOpen && isMoving)
        {
            Debug.Log("Opening..");
            current_loc += open_vector;
            this.transform.Translate(open_vector, Space.Self);
        }
        if (isOpen && isMoving)
        {
            Debug.Log("Closing..");
            current_loc += close_vector;
            this.transform.Translate(close_vector, Space.Self);
        }
        if (current_loc.x <= -5.99f)
        {
            isMoving = false;
            isOpen = true;
            Debug.Log("Is Moving: " + isMoving);
        }
        /*if (current_loc.x <= -5.99f)
        {
            isMoving = false;
            isOpen = false;
        }*/

        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        if (distance < 50.0f && activate_passage_action.GetState(right_hand))
        {
            Debug.Log("Open Door");
            if (current_loc.x >= -1.57f)
            {
                Debug.Log("Motion Activated");
                isMoving = true;
                isOpen = false;
            }
            if (current_loc.x <= -5.99f)
            {
                isMoving = true;
                isOpen = true;
            }
            if (audio != null)
            {
                audio_source.PlayOneShot(audio, 0.5f);
            }
        }
    }

   /* void OnMouseDown()
    {
        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        *//*Debug.Log("Mouse isOpen: " + isOpen);
        Debug.Log("Mouse isMoving: " + isMoving);
        Debug.Log("Mouse Current_Loc_z: " + current_loc.z);*//*
        if (distance > 5.0f)
        {
            //Debug.Log("Too Far");
            return;
        }
        if (current_loc.x >= 1.57f)
        {
            isMoving = true;
            isOpen = false;
        }
        if (current_loc.x <= -5.99f)
        {
            isMoving = true;
            isOpen = true;
        }
    }*/
}
