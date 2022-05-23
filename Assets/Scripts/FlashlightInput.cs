using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightInput : MonoBehaviour
{
    
    public event Action onFlashlightSwitchAction;
    
    public bool ReadSwitchFlashlightInput()
    {
        bool shouldSwitch = false;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            shouldSwitch = true;
            onFlashlightSwitchAction?.Invoke();
        }
        return shouldSwitch;
    }

    public Ray ReadMousePosition (Camera cam) 
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        return ray;
    }
}
