using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public event Action<bool> onPlayerRunningAction;


    public Vector2 ReadMovementInput()
    {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        float verticalAxis = Input.GetAxisRaw("Vertical");
        return new Vector2(horizontalAxis, verticalAxis);
    }

    public bool ReadRunningInput()
    {
        bool isRunning = false;
        if (Input.GetKey(KeyCode.LeftShift)) isRunning = true;
        else isRunning = false;
        if (ReadMovementInput() != Vector2.zero) onPlayerRunningAction?.Invoke(isRunning);
        else onPlayerRunningAction?.Invoke(false);
        return isRunning;
    }

    public bool ReadJumpingInput()
    {
        bool isJumping = false;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;

        }
        return isJumping;
    }
}
