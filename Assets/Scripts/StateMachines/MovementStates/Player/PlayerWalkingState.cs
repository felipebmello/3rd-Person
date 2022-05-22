using UnityEngine;

namespace MovementSystem.PlayerMovement {

    public class PlayerWalkingState : PlayerMovementState
    {
        public PlayerWalkingState(PlayerMovementStateMachine stateMachine) : base (stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public override void Enter()
        {
            movementSpeed = stateMachine.player.WalkingSpeed;
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void HandleInput()
        {
            base.HandleInput();
            WalkingTransition();
            JumpTransition();
            IdleTransition();
        }


        public void WalkingTransition()
        {
            if (movementInput != Vector2.zero)
            {
                if (runInput)
                {
                    stateMachine.ChangeState(stateMachine.runningState);
                }
            }
        }

        public override void Update()
        {
            base.Update();
        }

    }
}