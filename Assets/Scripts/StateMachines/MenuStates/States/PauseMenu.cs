using UnityEngine;
using UnityEngine.SceneManagement;

namespace MenuSystem {
    public class PauseMenu : MenuState
    {
        public PauseMenu(MenuStateMachine stateMachine) : base(stateMachine)
        {
            title = MenuTitle.PauseMenu;
        }
        public override void Enter () {
            base.Enter();
            Time.timeScale = 0;
        }
        public override void Exit (){
            base.Exit();
        }

        public override void ResumeGame()
        {
            Debug.Log ("You've resumed the game.");
            stateMachine.ChangeState(stateMachine.gameMenuState);
        }
        public override void RestartGame()
        {
            Debug.Log ("You've restarted the game.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            stateMachine.ChangeState(stateMachine.mainMenuState);
        }
        public override void QuitGame()
        {
            Application.Quit();
        }

    }
}