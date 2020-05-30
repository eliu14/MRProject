using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class ControllerMovePlatforming : MonoBehaviour
{
    public CharacterController controller;
    public SteamVR_Input_Sources handLeft;
    public SteamVR_Input_Sources handRight;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean stepFwd;
    public SteamVR_Action_Boolean stepBck;
    public SteamVR_Action_Boolean jump;
    public SteamVR_Action_Vector2 trackRot;
    public SteamVR_Behaviour_Pose controllerPosLeft;
    public SteamVR_Behaviour_Pose controllerPosRight;

    static public Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);
    float velmag = 0.0f;
    static public float aoa = 0;
    static public float angle_x = 0.0f;
    static public float angle_y = 0.0f;
    static public float FLmag = 0.0f;
    static public float FDmag = 0.0f;

    static public Vector3 nose = new Vector3(1.0f, 0.0f, 0.0f);
    static public Vector3 upnose = new Vector3(0.0f, 1.0f, 0.0f);
    float thrust = 0.0f;
    public Transform groundCheck;
    public float collisionRadius = 0.5f;
    public LayerMask groundMask;

    public bool grounded;
    bool fwdcheck = false;
    bool bwdcheck = false;
    bool jumpcheck = false;

    public float speed = 10.1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        grounded = Physics.CheckSphere(groundCheck.position, collisionRadius, groundMask);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 actual_velocity;
        Vector3 move = new Vector3(0, 0, 0);
        Vector3 FG = new Vector3(0, -9.81f * Time.deltaTime, 0);
        if (grounded)
        {
            
            Vector3 jumpImpulse = new Vector3(0.0f, 0.0f, 0.0f);
            bool fwdPressed = stepFwd.GetState(handLeft);
            bool bwdPressed = stepBck.GetState(handLeft);
            bool jumpPressed = jump.GetState(handRight);
            float w = 0.0f;

            //if (fwdPressed == true && fwdcheck == false) w = 2.0f;
            //if (bwdPressed == true && bwdcheck == false) w = -2.0f;
            if (jumpPressed == true && jumpcheck == false)
            {
                jumpImpulse = (Camera.main.transform.forward * 10) + new Vector3(0, 8, 0);
            }

            fwdcheck = fwdPressed;
            bwdcheck = bwdPressed;
            jumpcheck = jumpPressed;

            Vector2 tracking = new Vector2(0, w);
            Vector2 camRot = trackRot.GetAxis(handRight);
            Vector3 euler = new Vector3(0, camRot.x * Time.deltaTime * 70.0f, 0);
            //Debug.Log(camRot);

            x = tracking.x;
            z = tracking.y;

            move = Camera.main.transform.right * x + Camera.main.transform.forward * z + jumpImpulse;
            //move *= speed;
            this.transform.Rotate(euler);
        }
        actual_velocity = move + FG;
        controller.Move(actual_velocity); // pos = pos + move
    }

}
