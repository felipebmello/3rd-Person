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
    [SerializeField] bool _isStationary;
    [SerializeField] Material chasingMat;
    [SerializeField] Material idleMat;
    [SerializeField] Flashlight flashlight;
    [field: SerializeField] public float coverRadius = 20f;

    [field: Range(-1, 1)]
    [field: SerializeField] public float hideSensitivity = -0.5f;
    [field: SerializeField] public float minPlayerDistanceToCollider = 1f;
    public Collider[] colliders = new Collider[10];
    public bool hitByLight { get; private set; }
    public NavMeshAgent agent { get; private set; }
    public  FieldOfView fov { get; private set; }
    private bool seenPlayer;
    private float _smothRotationVelocity;
    private MeshRenderer _mesh;
    private Vector3 _hidingSpot;
    private Node topNode;


    void Awake() 
    {
        // Stores reference to the NavMeshAgent component on the Enemy game object (this)
        agent = gameObject.GetComponent<NavMeshAgent>();
        fov = gameObject.GetComponent<FieldOfView>();
        _targets = new Stack<Vector3>();
        _mesh = gameObject.GetComponentInChildren<MeshRenderer>();
        
    }
    private void OnEnable() 
    {
        
        if (afraidOfLight) {
            flashlight.onEnemyHitByLightAction += RunFromTarget;
            fov.onTargetWithinRangeAction += LookForTarget;
        }
        else {
            flashlight.onEnemyHitByLightAction += LookForTarget;
        }
    }

    private void OnDisable() 
    {
        if (afraidOfLight)  {
            fov.onTargetWithinRangeAction -= LookForTarget;
            flashlight.onEnemyHitByLightAction -= RunFromTarget;
        }
        else flashlight.onEnemyHitByLightAction -= LookForTarget;
    }
    // Start is called before the first frame update
    void Start()
    {
        _mesh.material = idleMat;
        InitializeWaypointStack();
        StartCoroutine(fov.CheckWithDelay());
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
        if (!afraidOfLight) hitByLight = true;
        _isStationary = false;
        if (seenPlayer) 
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
        seenPlayer = true;
    }

    private void RunFromTarget(Vector3 target)
    {
        if (afraidOfLight) hitByLight = true;
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
        agent.SetDestination(_targets.Peek());   
    }

    private void RotateWithMovement()
    {
        Vector3 angleDir = new Vector3(agent.velocity.x, 0f, agent.velocity.z).normalized;

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
        seenPlayer = false;
        _mesh.material = idleMat;
        if (_targets.Count == 0) InitializeWaypointStack();
    }
    public void OutOfSight()
    {
        if (!seenPlayer) 
        {
            _mesh.material = idleMat;
        }
        hitByLight = false;
        
    }
    public void Chase(Vector3 position)
    {
        _mesh.material = chasingMat;
        seenPlayer = true;
        agent.SetDestination(position);
    }

    public int ColliderArraySortComparer(Collider A, Collider B)
    {
        if (A == null && B != null) return 1;
        if (A != null && B == null) return -1;
        if (A == null && B == null) return 0;
        return Vector3.Distance(transform.position, A.transform.position).CompareTo(Vector3.Distance(transform.position, B.transform.position));
    }

    public void SetHidingSpot (Vector3 position) {
        _hidingSpot = position;
    }
    public Vector3 GetHidingSpot () {
        return _hidingSpot;
    }
}
