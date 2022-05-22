using UnityEngine;

namespace MovementSystem {
    public interface StateMachine
    {
        void ChangeState (MovementState newState);
    }

    public abstract class MovementStateMachine : StateMachine
    {
        private MovementState state;


        public void Enter() => state?.Enter();
        public void Exit() => state?.Exit();
        public void HandleInput() => state?.HandleInput();
        public void Update() => state?.Update();
        public void OnControllerColliderHit(ControllerColliderHit hit) => state?.OnControllerColliderHit(hit);

        public void ChangeState(MovementState newState)
        {
            state?.Exit();
            state = newState;
            state.Enter();
        }
    }
}