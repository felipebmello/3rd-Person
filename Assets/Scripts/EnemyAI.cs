using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    
    [SerializeField] List<Transform> patrolWaypoints;
    [SerializeField] bool chaseLight = true;
    // Parameter used on the SmoothDampAngle() function to store current velocity during each call
    [SerializeField] float smoothRotationTime = 0.2f;
    
    [SerializeField] Stack<Vector3> _targets;
    private MeshRenderer _mesh;
    [SerializeField] bool _isStationary;
    [SerializeField] Material chasingMat;
    [SerializeField] Material idleMat;
    private NavMeshAgent _agent;
    private EnemyFieldOfView _fov;
    private bool _seenPlayer;
    private float _smothRotationVelocity;


    void Awake() 
    {
        // Stores reference to the NavMeshAgent component on the Enemy game object (this)
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _fov = gameObject.GetComponent<EnemyFieldOfView>();
        _targets = new Stack<Vector3>();
        _mesh = gameObject.GetComponentInChildren<MeshRenderer>();
    }
    private void OnEnable() 
    {
        _fov.onTargetWithinRangeAction += LookForTarget;
    }
    private void OnDisable() 
    {
        _fov.onTargetWithinRangeAction -= LookForTarget;
    }
    // Start is called before the first frame update
    void Start()
    {
        _mesh.material = idleMat;
        InitializeWaypointStack();
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
        }
        else 
        {
            _targets.Push(transform.position);
            _mesh.material = chasingMat;
        }
        _targets.Push(target);
        _seenPlayer = true;
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
        _targets.Pop();
        _seenPlayer = false;
        _mesh.material = idleMat;
        if (_targets.Count == 0) InitializeWaypointStack();
    }
}
