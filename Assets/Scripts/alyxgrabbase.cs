using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class alyxgrabbase : MonoBehaviour
{
    public SteamVR_Behaviour_Pose VRControllerPose;

    List<float> anglelist = new List<float>();

    static public float current_velocity = 0.0f;
    void update_angle(float angle)
    {
        anglelist.Add(angle);
        if (anglelist.Count > 5)
            anglelist.RemoveAt(0);
    }
    void calc_angle_vel()
    {
        float av = 0;
        for (int i = 0; i < (anglelist.Count - 1); i++)
            av += anglelist[i + 1] - anglelist[i];
        av /= (float)anglelist.Count;
        current_velocity = av;
    }
    // Update is called once per frame
    void Update()
    {
        Quaternion rot = VRControllerPose.transform.rotation;
        Matrix4x4 m = Matrix4x4.Rotate(rot);
        Vector3 pointDirection = m.MultiplyPoint3x4(new Vector3(0, 0, 1));
        float angle = Vector3.Dot(pointDirection, new Vector3(0, 1, 0));
        update_angle(angle);
        calc_angle_vel();
    }
}
