using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    // Reference to the main camera, used to determine the forward direction of the player
    [field: Header("Camera Transform Reference")]
    [field: SerializeField] public Camera camera { get; private set; }
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float coneRadiusModifier = 4f;
    
    public FlashlightInput input {get; private set; }
    public FieldOfView playerFOV {get; protected set; }
    public bool IsOn { get => isOn; private set => isOn = value; }
    public RaycastHit hit {get; private set; }
    public bool isHittingPlayer {get; private set; }
    public event Action<Vector3> onEnemyHitByLightAction;
    public event Action onEnemyLeftLightAction;

    private bool isOn = false;

    private Ray ray;

    void Awake()
    {
        input = gameObject.GetComponent<FlashlightInput>();
        playerFOV = gameObject.GetComponentInParent<FieldOfView>();
    }

    void Update()
    {
        if (Time.timeScale > 0f)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        if (input.ReadSwitchFlashlightInput()) SwitchFlashlight();
        if (IsOn)
        {
            ray = input.ReadMousePosition(camera);
            RotateFlashlightToAngle();
            CreateSpotlightFOV();
        }
    }

    private void CreateSpotlightFOV()
    {
        if (Physics.Raycast(transform.position, 
                            transform.forward, 
                            out RaycastHit hit, playerFOV.obstructionMask))
        {
            if (hit.distance > 20f) 
            {
                onEnemyLeftLightAction?.Invoke();
                return;
            }
            this.hit = hit;
            FieldOfViewServices spotFOV = new FieldOfViewServices(
                                        this.hit.distance / coneRadiusModifier,
                                        360,
                                        playerFOV.targetMask,
                                        playerFOV.obstructionMask);
            if (spotFOV.CheckFOV(hit.point, transform.forward)) {
                onEnemyHitByLightAction?.Invoke(transform.parent.transform.position);
                Debug.Log("Hit "+spotFOV.GetTarget().name+"!!!");
                return;
            }  
        }
        onEnemyLeftLightAction?.Invoke();
    }

    

    private void RotateFlashlightToAngle()
    {
        Vector3 targetPosition = ray.direction;
        float targetAngle = Vector3.Angle(transform.parent.transform.forward, targetPosition);
        if (targetAngle < playerFOV.viewAngle / 2)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void SwitchFlashlight()
    {
        isOn = !isOn;
        foreach (Light l in GetComponentsInChildren<Light>())
        {
            l.enabled = !l.enabled;
        }
    }
    
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (hit.point, hit.distance / coneRadiusModifier);
        
    }

}
