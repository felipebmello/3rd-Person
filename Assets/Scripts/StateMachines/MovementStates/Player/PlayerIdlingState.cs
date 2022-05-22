namespace MovementSystem.PlayerMovement {

    public class PlayerIdlingState : PlayerMovementState
    {
        public PlayerIdlingState(PlayerMovementStateMachine stateMachine) : base (stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public override void Enter()
        {
            gravity = stateMachine.player.Gravity;
            base.Enter();
            //Start Idle Animation
        }

        public override void Exit()
        {
            base.Exit();
            //Stop Idle Animation
        }

        public override void HandleInput()
        {
            base.HandleInput();
            MovingTransition();
            JumpTransition();
        }

        public override void Update()
        {
            base.Update();
        }
        
    }
}