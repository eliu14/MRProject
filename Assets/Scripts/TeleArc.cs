using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class TeleArc : MonoBehaviour
{
    public CharacterController controller;
    public SteamVR_Input_Sources handLeft;
    public SteamVR_Action_Boolean teleButton;
    public SteamVR_Behaviour_Pose VRcontrlPose;
    public int segments = 12;
    public float maxdist = 20.0f;
    public LineRenderer lineRenderer;
    public Material defaultMaterial;
    public Material badMaterial;
    bool btn_pressed = false;
    bool target_locked = false;
    Vector3 target;
    public bool holding = false;
    // Update is called once per frame
    void Update()
    {
        bool telpressed = teleButton.GetState(handLeft);
        if (!telpressed && !btn_pressed)
        {
            lineRenderer.enabled = false;
            return;
        }
        if (!telpressed && btn_pressed)
        {
            btn_pressed = false;
            if (target_locked)
            {
                //Vector3 targetAbove = new Vector3(target.x, target.y + 2.58f, target.z);
                Vector3 diff = target - controller.transform.position;
                controller.Move(diff);
            }
            target_locked = false;
            lineRenderer.enabled = false;
            return;
        }
        btn_pressed = telpressed;
        var pointlist = new List<Vector3>();

        Quaternion rot = VRcontrlPose.transform.rotation;
        Matrix4x4 m = Matrix4x4.Rotate(rot);
        Vector3 pointDirection = m.MultiplyPoint3x4(new Vector3(0, 0, 1));
        float angle = Vector3.Dot(new Vector3(0, 1, 0), pointDirection);
        float dist = Mathf.Sin(angle * Mathf.PI) * maxdist;
        Vector3 dir = pointDirection;
        dir.y = 0;
        dir = Vector3.Normalize(dir) * dist;
        Vector3 A, P, B;
        A = VRcontrlPose.transform.position;
        B = A + dir;
        lineRenderer.enabled = true;
        // Does the ray interact with any objects excluding player layer
        RaycastHit hit;
        if (Physics.Raycast(B, new Vector3(0, -1, 0), out hit, 10))
        {
            B.y = B.y - hit.distance;
            target = B;
            //Debug.Log("Height Diff: " + (target.y - (controller.transform.position.y - 2.58f)));
            if (target.y - (controller.transform.position.y - 2.58f) <= 2.58f) target_locked = true;
            else target_locked = false;
            //target_locked = true;
            //Debug.Log("Target: " + target);
        }
        else
        {
            target_locked = false;
        }
        if (target_locked) lineRenderer.material = defaultMaterial;
        else lineRenderer.material = badMaterial;
        P = (A + B) / 2.0f;
        P.y += 3.0f;

        for (float t = 0; t <= 1.0f; t += 1.0f / (float)segments)
        {
            Vector3 X = A * (1.0f - t) + P * t;
            Vector3 Y = Vector3.Lerp(P, B, t);
            Vector3 R = Vector3.Lerp(X, Y, t);
            pointlist.Add(R);
        }
        lineRenderer.positionCount = pointlist.Count;
        lineRenderer.SetPositions(pointlist.ToArray());
    }
}
