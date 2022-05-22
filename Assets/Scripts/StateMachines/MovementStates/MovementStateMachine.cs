using UnityEngine;

namespace MovementSystem.PlayerMovement {
    public interface StateMachine
    {
        void ChangeState (MovementState newState);
    }

    public abstract class MovementStateMachine : StateMachine
    {
        private MovementState state;

        public MovementState State { get => state; set => state = value; }

        public void Enter() => State?.Enter();
        public void Exit() => State?.Exit();
        public void HandleInput() => State?.HandleInput();
        public void Update() => State?.Update();
        public void OnControllerColliderHit(ControllerColliderHit hit) => State?.OnControllerColliderHit(hit);

        public void ChangeState(MovementState newState)
        {
            State?.Exit();
            State = newState;
            State.Enter();
        }
    }
}