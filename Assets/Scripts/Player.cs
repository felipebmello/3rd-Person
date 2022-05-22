using UnityEngine;
using MovementSystem.PlayerMovement;
using System;

public class Player : MonoBehaviour
{
    // Reference to the main camera, used to determine the forward direction of the player
    [field: Header("Camera Transform Reference")]
    [field: SerializeField] public Transform Camera { get; private set; }


    [field: Header("Walking Variables")]
    [field: SerializeField, Range (0f, 10f)]  private float _walkingSpeed = 6f;
    [field: SerializeField, Range (0f, 2f)] private float _smoothRotationTime = 0.2f;


    [field: Header("Running Variables")]
    [field: SerializeField, Range (0f, 20f)] private float _runningSpeed = 10f;
    [field: Header("Jumping Variables")]
    [field: SerializeField, Range (0f, 20f)]  private float _jumpSpeed = 2f;
    [field: SerializeField, Range (0f, 10f)] private float _gravity = 9.81f;
 
 
    public CharacterController controller {get; private set; }
    public PlayerInput input {get; private set; }
    public float WalkingSpeed { get => _walkingSpeed; private set => _walkingSpeed = value; }
    public float SmoothRotationTime { get => _smoothRotationTime; set => _smoothRotationTime = value; }
    public float RunningSpeed { get => _runningSpeed; set => _runningSpeed = value; }
    public float JumpSpeed { get => _jumpSpeed; set => _jumpSpeed = value; }
    public float Gravity { get => _gravity; set => _gravity = value; }

    public event Action onPlayerWinConditionAction;
    public event Action onPlayerLoseConditionAction;
    
    private PlayerMovementStateMachine stateMachine;

    void Awake() 
    {
        // Stores reference to the Character Controller component on the Player game object (this)
        controller = gameObject.GetComponent<CharacterController>();
        stateMachine = new PlayerMovementStateMachine(this);
        input = gameObject.GetComponent<PlayerInput>();

    }

    // Start is called before the first frame update
    void Start()
    {
        stateMachine.ChangeState(stateMachine.idlingState);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0f)
        {
            stateMachine.HandleInput();
            stateMachine.Update();
        }
    }

    
    private void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.gameObject.CompareTag("Goal")) {
            onPlayerWinConditionAction?.Invoke();
        }
        stateMachine.OnControllerColliderHit(hit);
    } 
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Enemy")) {
            onPlayerLoseConditionAction?.Invoke();
        }
    }

    
}
