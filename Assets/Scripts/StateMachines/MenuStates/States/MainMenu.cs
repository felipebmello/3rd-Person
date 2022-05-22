using UnityEngine;

namespace MenuSystem {
    public class MainMenu : MenuState
    {
        public MainMenu(MenuStateMachine stateMachine) : base(stateMachine)
        {
            title = MenuTitle.MainMenu;
        }

        public override void Enter () {
            base.Enter();
            Time.timeScale = 0;

        }
        public override void Exit (){
            base.Exit();
        }
        public override void StartGame()
        {            
            Debug.Log ("You've started the game.");
            stateMachine.ChangeState(stateMachine.gameMenuState);
        }
        public override void QuitGame()
        {
            Debug.Log ("You've quit the game.");
            Application.Quit();
        }
    }
}
