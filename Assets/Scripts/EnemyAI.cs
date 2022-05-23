using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    
    [SerializeField] List<Transform> patrolWaypoints;
    [SerializeField] bool afraidOfLight = true;
    // Parameter used on the SmoothDampAngle() function to store current velocity during each call
    [SerializeField] float smoothRotationTime = 0.2f;
    
    [SerializeField] Stack<Vector3> _targets;
    private MeshRenderer _mesh;
    [SerializeField] bool _isStationary;
    [SerializeField] Material chasingMat;
    [SerializeField] Material idleMat;
    [SerializeField] Flashlight flashlight;
    private NavMeshAgent _agent;
    private FieldOfView _fov;
    private bool _seenPlayer;
    private bool _isAfraid = false;
    private float _smothRotationVelocity;


    void Awake() 
    {
        // Stores reference to the NavMeshAgent component on the Enemy game object (this)
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _fov = gameObject.GetComponent<FieldOfView>();
        _targets = new Stack<Vector3>();
        _mesh = gameObject.GetComponentInChildren<MeshRenderer>();
        
    }
    private void OnEnable() 
    {
        
        if (afraidOfLight) {
            flashlight.onEnemyHitByLightAction += RunFromTarget;
            _fov.onTargetWithinRangeAction += LookForTarget;
        }
        else {
            flashlight.onEnemyHitByLightAction += LookForTarget;
        }
    }

    private void OnDisable() 
    {
        if (afraidOfLight)  {
            _fov.onTargetWithinRangeAction -= LookForTarget;
            flashlight.onEnemyHitByLightAction -= RunFromTarget;
        }
        else flashlight.onEnemyHitByLightAction -= LookForTarget;
    }
    // Start is called before the first frame update
    void Start()
    {
        _mesh.material = idleMat;
        InitializeWaypointStack();
        StartCoroutine(_fov.CheckWithDelay());
    }

    private void InitializeWaypointStack()
    {
        if (!_isStationary && patrolWaypoints.Count > 0) 
        {   
            patrolWaypoints.Reverse();
            foreach (Transform waypoint in patrolWaypoints)
            {
                _targets.Push(waypoint.position);
            }
        }
        else 
        {
            _isStationary = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
    }

    private void LookForTarget(Vector3 target)
    {
        _isStationary = false;
        if (_seenPlayer) 
        {
            _targets.Pop();
            Debug.Log(_targets.Peek());
        }
        else 
        {
            _targets.Push(transform.position);
            _mesh.material = chasingMat;
        }
        Debug.Log(" current Position "+transform.position);
        _targets.Push(target);
        _seenPlayer = true;
    }

    private void RunFromTarget(Vector3 target)
    {
        Debug.Log("RunFromTarget Behaviour not implemented!");
        /*if (_isAfraid) return;
        if (!_seenPlayer) _isAfraid = true;
        if (_seenPlayer) 
        {
            _targets.Pop();
        }
        _mesh.material = idleMat;*/
    }

    private void EnemyMovement()
    {
        if (_isStationary) return;
        UpdateDestination();
        RotateWithMovement();
        if (Vector3.Distance(gameObject.transform.position, _targets.Peek()) < 1f)
        {
            NextWaypoint();
        }
    }
    private void UpdateDestination()
    {
        _agent.SetDestination(_targets.Peek());   
    }

    private void RotateWithMovement()
    {
        Vector3 angleDir = new Vector3(_agent.velocity.x, 0f, _agent.velocity.z).normalized;

        if (angleDir.magnitude > 0f)
        {
            float targetAngle = Mathf.Atan2(angleDir.x, angleDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref _smothRotationVelocity,
                smoothRotationTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private void NextWaypoint() 
    {
        Debug.Log(_targets.Peek()+" current Position "+transform.position);
        _targets.Pop();
        _seenPlayer = false;
        _mesh.material = idleMat;
        if (_targets.Count == 0) InitializeWaypointStack();
    }
}
