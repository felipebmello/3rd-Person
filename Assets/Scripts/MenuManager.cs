using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MenuSystem {
    
    public enum MenuTitle
    {
        MainMenu, 
        GameMenu, 
        PauseMenu,
        VictoryMenu
    }

    public class MenuManager : MonoBehaviour
    {
        [field: Header("UI Canvas Reference")]
        [field: SerializeField] public Canvas[] UICanvas { get; private set; }
        [field: SerializeField] public TMP_Text flashlightUI { get; private set; }
        
        private MenuStateMachine stateMachine;
        



        void Awake()
        {
            stateMachine = new MenuStateMachine(this);
            FindObjectOfType<FlashlightController>().onFlashlightSwitchAction += OnFlashlightToggle;
            FindObjectOfType<Player>().onPlayerWinConditionAction += OnWinningLevel;
            FindObjectOfType<Player>().onPlayerLoseConditionAction += OnLosingLevel;
        }

        void Start() {
            stateMachine.ChangeState(stateMachine.mainMenuState);
        }

        void Update() {
            ProcessEscapeInput();
            
        }

        private void ProcessEscapeInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                stateMachine.PauseGame();
            }
        }

        public virtual void OnStartButton () {
            stateMachine.StartGame();
        }
        public virtual void OnResumeButton () {
            stateMachine.ResumeGame();
        }
        public virtual void OnRestartButton () {
            stateMachine.RestartGame();
        }
        public virtual void OnQuitButton () {
            stateMachine.QuitGame();
        }
        public virtual void OnWinningLevel () {
            stateMachine.WinGame();
        }public virtual void OnLosingLevel () {
            stateMachine.RestartGame();
        }
        public virtual void OnFlashlightToggle(bool isOn) {
            if (isOn) flashlightUI.color = Color.yellow;
            else flashlightUI.color = Color.white;
        }
    }
}