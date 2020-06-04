using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseHiddenPassage : MonoBehaviour
{
    public GameObject reactable;
    public GameObject player;
    bool isOpen = false;
    bool isMoving = false;
    Vector3 current_loc;
    Vector3 open_vector;
    Vector3 close_vector;

    // Start is called before the first frame update
    void Start()
    {
        current_loc = transform.localPosition;
        open_vector = new Vector3(0.0f, 0.0f, 0.001f);
        close_vector = new Vector3(0.0f, 0.0f, -0.001f);
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("Update isOpen: " + isOpen);
        //Debug.Log("Update isMoving: " + isMoving);
        //Debug.Log("Update Current_Loc_z: " + current_loc.z);
        if (!isOpen && isMoving)
        {
            //Debug.Log("Opening..");
            current_loc += open_vector;
            this.transform.Translate(open_vector, Space.Self);
        }
        if (isOpen && isMoving)
        {
            //Debug.Log("Closing..");
            current_loc += close_vector;
            this.transform.Translate(close_vector, Space.Self);
        }
        if (current_loc.z <= 5.635f)
        {
            isMoving = false;
            isOpen = true;

        }
        if (current_loc.z >= 12.12f)
        {
            isMoving = false;
            isOpen = false;
        }
    }

    void OnMouseDown()
    {
        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        /*Debug.Log("Mouse isOpen: " + isOpen);
        Debug.Log("Mouse isMoving: " + isMoving);
        Debug.Log("Mouse Current_Loc_z: " + current_loc.z);*/
        if (distance > 5.0f)
        {
            //Debug.Log("Too Far");
            return;
        }
        if (current_loc.z <= 5.635f)
        {
            isMoving = true;
            isOpen = false;
        }
        if (current_loc.z >= 12.12f)
        {
            isMoving = true;
            isOpen = true;
        }
    }
}
