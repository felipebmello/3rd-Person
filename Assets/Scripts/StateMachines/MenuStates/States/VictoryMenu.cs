using UnityEngine;
using UnityEngine.SceneManagement;

namespace MenuSystem {
    public class VictoryMenu : MenuState
    {
        public VictoryMenu(MenuStateMachine stateMachine)  : base (stateMachine)
        {

            title = MenuTitle.VictoryMenu;
        }
        
        public override void Enter () {
            base.Enter();
            Time.timeScale = 0;
        }
        public override void Exit (){
            base.Exit();
        }
        public override void RestartGame()
        {
            Debug.Log ("You've restarted the game.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            stateMachine.ChangeState(stateMachine.mainMenuState);
        }
        public override void QuitGame()
        {
            Debug.Log ("You've quit the game.");
            Application.Quit();
        }

    }
}