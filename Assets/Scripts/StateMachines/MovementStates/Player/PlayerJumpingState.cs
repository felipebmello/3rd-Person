using System;
using UnityEngine;

namespace MovementSystem.PlayerMovement {

    public class PlayerJumpingState : PlayerMovementState
    {
        public PlayerJumpingState(PlayerMovementStateMachine stateMachine) : base (stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public override void Enter()
        {
            verticalSpeed = stateMachine.player.JumpSpeed;
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void HandleInput()
        {
            base.HandleInput();
            movementSpeed = CurrentMovementSpeed();
        }

        private float CurrentMovementSpeed()
        {
            if (runInput) return stateMachine.player.RunningSpeed;
            else return stateMachine.player.WalkingSpeed;
        }

        public override void Update()
        {
            base.Update();
        }

        public override void OnControllerColliderHit(ControllerColliderHit hit) 
        {
            if (hit.gameObject.CompareTag("Ground")) {
                IdleTransition();
                MovingTransition();
            }
        }

    }
}