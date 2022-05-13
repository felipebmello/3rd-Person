using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineInputAxis : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CinemachineCore.GetInputAxis = GetAxisMouseKey1;
    }

        //returns normal call if Mouse 1 (right button) is pressed
    public float GetAxisMouseKey1(string axisName) 
    {
        if (axisName == "Mouse X" || axisName == "Mouse Y") 
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                //returns normal call if the button is pressed
                return UnityEngine.Input.GetAxis(axisName);
            } 
            else
            {
                return 0;
            }
        }
        //returns normal call if axisName is not either "Mouse X" or "Mouse Y"
        return UnityEngine.Input.GetAxis(axisName);
    }
}
