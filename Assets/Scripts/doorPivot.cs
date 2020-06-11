using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class doorPivot : MonoBehaviour
{
    public GameObject player;

    public SteamVR_Input_Sources left_hand;
    public SteamVR_Action_Boolean open_door_action;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float mom_ang = 0.0f;
        if (move == 1)
            mom_ang = 0.5f;
        if (move == -1)
            mom_ang = -0.5f;

        Vector3 pos = GameObject.Find("doorPivotLeft").transform.position;
        this.transform.RotateAround(pos, new Vector3(0, 1, 0), mom_ang);//(pos, new Vector3(0, 1, 0), mom_ang);

        angley += mom_ang;

        if (angley <= 0.00f) move = 0;
        if (angley >= 90.0f) move = 0;

        float distance = Vector3.Distance(this.transform.position, player.transform.position);

        if (distance < 10.0f && open_door_action.GetState(left_hand))
        {
            if (angley <= 0.00f) move = 1;
            if (angley >= 90.0f) move = -1;
        }
    }

    float angley = 0.0f;
    int move = 0; // 1 .. open, -1 .. close
}
