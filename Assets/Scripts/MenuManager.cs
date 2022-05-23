using UnityEngine;
using UnityEngine.UI;

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
        [field: SerializeField] public Image flashlightOnUI { get; private set; }
        [field: SerializeField] public Player player { get; private set; }
        [field: SerializeField] public Flashlight flashlight { get; private set; }
        
        private MenuStateMachine stateMachine;
        private bool _isFlashlightOn;
        
        void Awake()
        {
            stateMachine = new MenuStateMachine(this);
            player.onPlayerWinConditionAction += OnWinningLevel;
            player.onPlayerLoseConditionAction += OnLosingLevel;
        }

        void Start() {
            flashlight.input.onFlashlightSwitchAction += OnFlashlightToggle;
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
        public virtual void OnFlashlightToggle() {
            _isFlashlightOn = !_isFlashlightOn;
            flashlightOnUI.gameObject.SetActive(_isFlashlightOn);
        }
    }
}