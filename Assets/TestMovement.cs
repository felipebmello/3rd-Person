using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    [SerializeField] private float _walkingSpeed = 10f;
    [SerializeField] private float _runningSpeed = 20f;
    [SerializeField] private float _jumpSpeed = 10f;
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private bool isGrounded = false;
    private float vSpeed;
    private CharacterController _controller;
    // Start is called before the first frame update
    void Start()
    {
        _controller = gameObject.GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        float verticalAxis = Input.GetAxisRaw("Vertical");
        float movementSpeed = _walkingSpeed;
        if (isGrounded)
        {
            vSpeed = 0;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movementSpeed = _runningSpeed;

            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                vSpeed = _jumpSpeed;
                isGrounded = false;
            }
        }
        
        vSpeed -= _gravity * Time.deltaTime;

        Vector3 moveDir = new Vector3(horizontalAxis, 0f, verticalAxis).normalized;
        moveDir.y = vSpeed;

        _controller.Move(moveDir * movementSpeed * Time.deltaTime);
        
    }
    
    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.collider.gameObject.tag.Equals("Ground")) {
            isGrounded = true;
        }
        
    }

}
