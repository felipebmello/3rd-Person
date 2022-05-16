using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    // Reference to the main camera, used to determine the forward direction of the player
    [SerializeField] Transform cam;
    [SerializeField] float walkingSpeed = 6f;
    [SerializeField] float runningSpeed = 10f;
    [SerializeField] float smoothRotationTime = 0.2f;
    private CharacterController _controller;
    // Parameter used on the SmoothDampAngle() function to store current velocity during each call
    private float _smothRotationVelocity;
    private bool _isRunning;

    void Awake() 
    {
        // Stores reference to the Character Controller component on the Player game object (this)
        _controller = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // GetAxisRaw() returns either {-1f, 0f, 1f}, GetAxis() gradually returns a float between -1 and 1 (works better for joysticks). 
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        float verticalAxis = Input.GetAxisRaw("Vertical");
        float movementSpeed = walkingSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            _isRunning = true;
            movementSpeed = runningSpeed;
        }
        ProcessPlayerMovement(horizontalAxis, verticalAxis, movementSpeed);

    }

    private void ProcessPlayerMovement(float horizontalAxis, float verticalAxis, float movementSpeed)
    {
        // Normalized to avoid adding up velocity during diagonal movement
        Vector3 angleDir = new Vector3(horizontalAxis, 0f, verticalAxis).normalized;

        // If there is movement in any direction
        if (angleDir.magnitude > 0f)
        {
            // Atan2 returns the correct angle {0° to 360°, in Radians} of the movement in order to properly rotate the player in the correct direction
            // Adds the camera rotation so the player rotates in relation to the direction the camera faces
            float targetAngle = Mathf.Atan2(angleDir.x, angleDir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            // Smooths the transition between current and target angles
            float angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref _smothRotationVelocity,
                smoothRotationTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Creates a new direction vector for movement, based on the previous resulting angle
            // Multiplying a rotation with a Vector3.forward results in another Vector3
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            // Normalized to avoid adding up velocity during diagonal movement
            _controller.Move(moveDir.normalized * movementSpeed * Time.deltaTime);
        }
    }
}
