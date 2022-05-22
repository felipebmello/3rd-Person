using System;
using UnityEngine;

namespace MovementSystem.PlayerMovement {
    public class PlayerMovementState : MovementState
    {
        protected PlayerMovementStateMachine stateMachine;
        protected Vector2 movementInput;
        protected float movementSpeed;
        protected float verticalSpeed;
        protected float gravity;
        protected bool jumpInput;
        protected bool runInput;
        private float _smothRotationVelocity;


        public PlayerMovementState (PlayerMovementStateMachine stateMachine) 
        {
            this.stateMachine = stateMachine;
        }

       

        public virtual void Enter()
        {
            gravity = stateMachine.player.Gravity;
            Debug.Log("State "+GetType().Name);
        }

        public virtual void Exit()
        {
        }

        public virtual void HandleInput()
        {
            ReadMovementInput();
            ReadJumpingInput();
            ReadRunningInput();
        }

        public virtual void Update()
        {
            Move();
        }

        public virtual void OnControllerColliderHit(ControllerColliderHit hit) 
        {
            if (hit.gameObject.CompareTag("Ground")) {
                verticalSpeed = 0;
            }
        }

        private void ReadMovementInput()
        {
            movementInput = stateMachine.player.input.ReadMovementInput();
        }
        private void ReadJumpingInput()
        {
            jumpInput = stateMachine.player.input.ReadJumpingInput();
        }
        private void ReadRunningInput()
        {
            runInput = stateMachine.player.input.ReadRunningInput();
        }
        public void IdleTransition()
        {
            if (movementInput == Vector2.zero) stateMachine.ChangeState(stateMachine.idlingState);
        }
        
        public void MovingTransition()
        {
            if (movementInput != Vector2.zero)
            {
                if (runInput)
                {
                    stateMachine.ChangeState(stateMachine.runningState);
                }
                else
                {
                    stateMachine.ChangeState(stateMachine.walkingState);
                }
            }
        }
        
        public void JumpTransition()
        {
            if (jumpInput) stateMachine.ChangeState(stateMachine.jumpingState);
        }

        private void Move()
        {
            if (verticalSpeed == 0 && movementInput == Vector2.zero)
            {
                //stateMachine.ChangeState(stateMachine.idlingState);
                return;
            }
            verticalSpeed -= gravity * Time.deltaTime;

            // Normalized to avoid adding up velocity during diagonal movement
            Vector3 moveDir = new Vector3(movementInput.x, 0f, movementInput.y).normalized;
            if (moveDir.magnitude > 0f)
            {
                // Atan2 returns the correct angle {0° to 360°, in Radians} of the movement in order to properly rotate the player in the correct direction
                // Adds the camera rotation so the player rotates in relation to the direction the camera faces
                float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + stateMachine.player.Camera.transform.eulerAngles.y;

                // Smooths the transition between current and target angles
                float angle = Mathf.SmoothDampAngle(
                    stateMachine.player.transform.eulerAngles.y,
                    targetAngle,
                    ref  _smothRotationVelocity,
                     stateMachine.player.SmoothRotationTime);
                stateMachine.player.transform.rotation = Quaternion.Euler(0f, angle, 0f);

                // Creates a new direction vector for movement, based on the previous resulting angle
                // Multiplying a rotation with a Vector3.forward results in another Vector3
                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            }
            moveDir.y = verticalSpeed;
            stateMachine.player.controller.Move(moveDir * movementSpeed * Time.deltaTime);
        }
    }
}