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

    public Transform groundCheck;
    public float collisionRadius = 0.5f;
    public LayerMask groundMask;

    public bool grounded;
    bool fwdcheck = false;
    bool bwdcheck = false;
    bool jumpcheck = false;

    Vector3 last_ground_pos;
    Transform contact_transform = null;
    bool contact = false;

    void FixedUpdate()
    {
        grounded = Physics.CheckSphere(groundCheck.position, collisionRadius, groundMask);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 actual_velocity;
        Vector3 move = new Vector3(0, 0, 0);
        Vector3 FG = new Vector3(0, -9.81f * Time.deltaTime, 0);

        Vector3 jumpImpulse = new Vector3(0.0f, 0.0f, 0.0f);
        bool fwdPressed = stepFwd.GetState(handLeft);
        bool bwdPressed = stepBck.GetState(handLeft);
        bool jumpPressed = jump.GetState(handRight);
        float w = 0.0f;

        if (fwdPressed == true && fwdcheck == false) w = 2.0f;
        if (bwdPressed == true && bwdcheck == false) w = -2.0f;
        if (jumpPressed == true && jumpcheck == false && grounded)
        {
            jumpImpulse = (Camera.main.transform.forward * 5) + new Vector3(0, 4, 0);
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
        //Collision check for moving platforms

        Vector3 parent_vel = new Vector3(0, 0, 0);

        RaycastHit hitDown;
        if (Physics.Raycast(controller.transform.position, Vector3.down, out hitDown))
        {
            if (hitDown.distance < 2.8f)
            {
                if (hitDown.transform != contact_transform)
                {
                    contact_transform = hitDown.transform;
                    contact = false;
                }
                //Debug.Log("HitDistance: " + hitDown.distance);
                if (contact == true)
                    parent_vel = hitDown.transform.position - last_ground_pos;
                last_ground_pos = hitDown.transform.position;
                contact = true;

            }
            else
            {
                contact = false;
            }
        }
        else
        {
            contact = false;
        }
        move = Camera.main.transform.right * x + Camera.main.transform.forward * z + jumpImpulse;

        this.transform.Rotate(euler);
        actual_velocity = move + FG + parent_vel;
        controller.Move(actual_velocity);
    }

}
