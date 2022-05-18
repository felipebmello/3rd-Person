using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonController : MonoBehaviour
{
    // Reference to the main camera, used to determine the forward direction of the player
    [SerializeField] Transform cam;
    [SerializeField] private float _walkingSpeed = 6f;
    [SerializeField] private float _runningSpeed = 10f;
    [SerializeField] private float _jumpSpeed = 2f;
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private float _smoothRotationTime = 0.2f;
    [SerializeField] private bool _isGrounded = false;

    public event Action<bool> onPlayerRunningAction;
    private CharacterController _controller;
    // Parameter used on the SmoothDampAngle() function to store current velocity during each call
    private float _smothRotationVelocity;
    private float _vSpeed;
    private bool _isRunning;
    private bool _isJumping;

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
        float movementSpeed = _walkingSpeed;
        if (_isGrounded)
        {
            _vSpeed = 0;
            movementSpeed = ProcessRunningInput(movementSpeed);
            ProcessJumpInput();
        }

        ProcessMovementInput(horizontalAxis, verticalAxis, movementSpeed);

    }

    private void ProcessJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _vSpeed = _jumpSpeed;
            _isGrounded = false;

        }
        else _isJumping = false;
    }

    private float ProcessRunningInput(float movementSpeed)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _isRunning = true;
            movementSpeed = _runningSpeed;

        }
        else _isRunning = false;
        onPlayerRunningAction.Invoke(_isRunning);
        return movementSpeed;
    }

    private void ProcessMovementInput(float horizontalAxis, float verticalAxis, float movementSpeed)
    {
        
        
        _vSpeed -= _gravity * Time.deltaTime;
        
        // Normalized to avoid adding up velocity during diagonal movement
        Vector3 moveDir = new Vector3(horizontalAxis, 0f, verticalAxis).normalized;

        // If there is movement in any direction
        if (moveDir.magnitude > 0f)
        {
            // Atan2 returns the correct angle {0° to 360°, in Radians} of the movement in order to properly rotate the player in the correct direction
            // Adds the camera rotation so the player rotates in relation to the direction the camera faces
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;

            // Smooths the transition between current and target angles
            float angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref _smothRotationVelocity,
                _smoothRotationTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Creates a new direction vector for movement, based on the previous resulting angle
            // Multiplying a rotation with a Vector3.forward results in another Vector3
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir.y = _vSpeed;
            // Normalized to avoid adding up velocity during diagonal movement
            _controller.Move(moveDir * movementSpeed * Time.deltaTime);
        }
        else {
            moveDir.y = _vSpeed;
            _controller.Move(moveDir * movementSpeed * Time.deltaTime);
        }
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.gameObject.CompareTag("Ground")) {
            _isGrounded = true;
        }
        
    }
}
