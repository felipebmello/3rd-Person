using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MenuSystem {
    public interface StateMachine
    {
        void ChangeState (MenuState newState);
    }
    public class MenuStateMachine : StateMachine
    {
        private MenuState state;
        private Stack<MenuState> statesStack = new Stack<MenuState>();
        public MenuManager menuManager {get;}
        public MainMenu mainMenuState {get;}
        public GameMenu gameMenuState {get;}
        public PauseMenu pauseMenuState {get;}
        public VictoryMenu victoryMenuState {get;}

        public MenuStateMachine(MenuManager menuManager)
        {
            this.menuManager = menuManager;
            mainMenuState = new MainMenu(this);
            gameMenuState = new GameMenu(this);
            pauseMenuState = new PauseMenu(this);
            victoryMenuState = new VictoryMenu(this);
        }
        public void StartGame() => state?.StartGame();
        public void ResumeGame() => state?.ResumeGame();
        public void PauseGame() => state?.PauseGame();
        public void RestartGame() => state?.RestartGame();
        public void QuitGame() => state?.QuitGame();
        public void WinGame() => state?.WinGame();
        public void Enter() => state?.Enter();
        public void Exit() => state?.Exit();


        public void ChangeState(MenuState newState)
        {   
            state?.Exit();
            state = newState;
            state.Enter();
        }
        public Canvas FindCanvas(MenuTitle title)
        {
            foreach (Canvas canvas in menuManager.UICanvas) 
            {
                if (canvas.name.Equals(title.ToString())) return canvas;
            }
            return null;
        }
    }
}