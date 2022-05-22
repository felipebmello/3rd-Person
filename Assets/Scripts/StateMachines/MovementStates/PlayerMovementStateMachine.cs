namespace MovementSystem.PlayerMovement {
    public class PlayerMovementStateMachine : MovementStateMachine
    {
        public Player player {get;}

        public PlayerIdlingState idlingState {get;}
        public PlayerWalkingState walkingState {get;}
        public PlayerRunningState runningState {get;}
        public PlayerJumpingState jumpingState {get;}
        
        public PlayerMovementStateMachine(Player player)
        {
            this.player = player;
            idlingState = new PlayerIdlingState(this);
            walkingState = new PlayerWalkingState(this);
            runningState = new PlayerRunningState(this);
            jumpingState = new PlayerJumpingState(this);
        }
    }
}

