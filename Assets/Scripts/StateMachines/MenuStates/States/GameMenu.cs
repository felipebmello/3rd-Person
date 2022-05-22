using UnityEngine;
using UnityEngine.SceneManagement;

namespace MenuSystem {
    public class GameMenu :  MenuState
    {
        public GameMenu(MenuStateMachine stateMachine) : base(stateMachine)
        {
            title = MenuTitle.GameMenu;
        }
        public override void Enter () {
            base.Enter();
            Time.timeScale = 1;
        }
        public override void Exit (){
            base.Exit();
        }
        public override void RestartGame()
        {
            Debug.Log ("You've lost the game.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            stateMachine.ChangeState(stateMachine.mainMenuState);
        }

        public override void PauseGame()
        {
            Debug.Log ("You've paused the game.");
            stateMachine.ChangeState(stateMachine.pauseMenuState);
        }
        public override void WinGame()
        {
            Debug.Log("You've won the game.");
            stateMachine.ChangeState(stateMachine.victoryMenuState);
        }
    }
}
