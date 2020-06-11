using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class alyxgrib : MonoBehaviour
{
    public SteamVR_Input_Sources handRight;
    public SteamVR_Action_Boolean grabbutton;
    public SteamVR_Action_Boolean grabsearchbutton;
    public SteamVR_Behaviour_Pose VRControllerPose;

    public LineRenderer linerenderer;
    public Rigidbody rb;

    bool btn_pressed = false;
    static bool we_are_locked = false;
    static public bool holding;
    static public Vector3 vel;
    static public float velmag;
    static public bool thrown = false;
    public bool nearPlayer = false;

    static public float dist = 0.0f;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        bool grabpressed = grabbutton.GetState(handRight);
        bool searchpressed = grabsearchbutton.GetState(handRight);
        vel = rb.velocity;
        velmag = vel.magnitude;
        dist = Vector3.Distance(VRControllerPose.transform.position, this.transform.position);
        if ((velmag > 1.0f) &&  dist < 2.0f && !thrown)
        {
            if (!holding) holding = true;
            
        }
        if ((velmag < 1.0f) && thrown && dist > 4.5f)
        {
            thrown = false;
            holding = false;
        }
        if (holding && !thrown)
        {
            linerenderer.enabled = false;
            this.transform.position = VRControllerPose.transform.position;
            this.transform.rotation = VRControllerPose.transform.rotation;
            rb.velocity = Vector3.zero;
            if (searchpressed)
            {
                
                holding = !holding;
                thrown = true;
            }
            if (!grabpressed && btn_pressed)
            {

                float v = alyxgrabbase.current_velocity;

                if (v > 0.02f)
                {
                    Vector3 a = VRControllerPose.transform.forward;
                    Vector3 ab = a + new Vector3(0, 1, 0);
                    rb.AddForce(ab * v * 6000.0f);
                    holding = !holding;
                    thrown = true;
                }
               
            }
            btn_pressed = grabpressed;
        }
        else
        {
           
            if (!searchpressed)
            {
                linerenderer.enabled = false;
                return;
            }
            if (grabpressed && we_are_locked == false)
            {
                linerenderer.enabled = false;
                return;
            }
            if (!grabpressed && btn_pressed)
            {
                btn_pressed = false;
                if (we_are_locked)
                {
                    float v = alyxgrabbase.current_velocity;
                    
                    if (v > 0.02f)
                    {
                        Vector3 a = VRControllerPose.transform.position;
                        Vector3 b = this.transform.position;
                        Vector3 ab = Vector3.Normalize(a - b) + new Vector3(0, 1, 0);
                        rb.AddForce(ab * v * 6000.0f);
                    }
                }
                we_are_locked = false;
                linerenderer.enabled = false;
                return;
            }
            btn_pressed = grabpressed;
            var pointlist = new List<Vector3>(); ;
            Quaternion rot = VRControllerPose.transform.rotation;
            Matrix4x4 m = Matrix4x4.Rotate(rot);
            Vector3 pointDirection = m.MultiplyPoint3x4(new Vector3(0, 0, 1));
            Vector3 A = VRControllerPose.transform.position;
            Vector3 B = this.transform.position;
            Vector3 AB = Vector3.Normalize(B - A);
            float d = Vector3.Dot(AB, pointDirection);
            if (d > .9 || (we_are_locked && grabpressed))
            {
                we_are_locked = true;
                linerenderer.enabled = true;
            }
            else
            {
                we_are_locked = false;
                linerenderer.enabled = false;
            }

            pointlist.Add(A);
            pointlist.Add(B);

            linerenderer.positionCount = pointlist.Count;
            linerenderer.SetPositions(pointlist.ToArray());
        }
        
    }
}
