using UnityEngine;
using UnityEngine.SceneManagement;

namespace MenuSystem {
    public abstract class MenuState {
        protected Canvas canvas;
        protected MenuTitle title;
        protected MenuStateMachine stateMachine;

        public MenuState (MenuStateMachine stateMachine) {
            this.stateMachine = stateMachine;
        }
        public virtual void Enter (){
            canvas = stateMachine.FindCanvas(title);
            canvas.gameObject.SetActive(true);
            Debug.Log("State "+GetType().Name);
        }
        public virtual void Exit (){
            canvas.gameObject.SetActive(false);
        }

        public virtual void StartGame () {
        }
        public virtual void ResumeGame (){
        }
        public virtual void PauseGame (){
        }
        public virtual void RestartGame (){
        }
        public virtual void QuitGame (){
        }
        public virtual void WinGame (){
        }
    }
}