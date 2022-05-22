using UnityEngine;

namespace MovementSystem.PlayerMovement {

    public class PlayerRunningState : PlayerMovementState
    {
        public PlayerRunningState(PlayerMovementStateMachine stateMachine) : base (stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public override void Enter()
        {
            base.Enter();
            movementSpeed = stateMachine.player.RunningSpeed;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void HandleInput()
        {
            base.HandleInput();
            RunningTransition();
            JumpTransition();
            IdleTransition();
        }


        public void RunningTransition()
        {
            if (movementInput != Vector2.zero)
            {
                if (!runInput)
                {
                    stateMachine.ChangeState(stateMachine.walkingState);
                }
            }
        }

        public override void Update()
        {
            base.Update();
        }
    }
}