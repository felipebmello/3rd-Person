using UnityEngine;

namespace MovementSystem {
    public interface MovementState
    {

        public void Enter();
        public void Exit();
        public void HandleInput();
        public void Update();
        public void OnControllerColliderHit(ControllerColliderHit hit);
        
    }
}
