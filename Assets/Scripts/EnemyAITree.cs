using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAITree : MonoBehaviour
{
    [field: SerializeField] protected Flashlight flashlight { get; private set; }
    [field: SerializeField] Material chasingMat;

    [field: SerializeField] Material idleMat;
    [field: SerializeField] List<Transform> patrolWaypoints;
    [field: SerializeField] float smoothRotationTime = 0.2f;
    [field: SerializeField] public float coverRadius = 20f;

    [field: Range(-1, 1)]
    [field: SerializeField] public float hideSensitivity = -0.5f;
    [field: SerializeField] public float minPlayerDistanceToCollider = 1f;
    public NavMeshAgent agent { get; private set; }
    public  FieldOfView fov { get; private set; }
    public Collider[] _colliders = new Collider[10];
    private Stack<Vector3> _targets;
    private Vector3 _playerLastKnownPosition;
    private Node _topNode;
    private bool _hitByLight = false;
    private MeshRenderer _mesh;
    private float _smothRotationVelocity;
    private Vector3 _hidingSpot;
    // Start is called before the first frame update
    void Awake() {
        agent = gameObject.GetComponent<NavMeshAgent>();
        fov = gameObject.GetComponent<FieldOfView>();
        _targets = new Stack<Vector3>();
        _mesh = gameObject.GetComponentInChildren<MeshRenderer>();
    }

    private void OnEnable() 
    {
        flashlight.onEnemyHitByLightAction += HandleLightHit;
        flashlight.onEnemyLeftLightAction += HandleLeftLight;
        fov.onTargetWithinRangeAction += TargetOnLineOfSight;
    }

    private void HandleLeftLight()
    {
        _hitByLight = false;
    }

    private void HandleLightHit(Vector3 playerPosition)
    {
        _hitByLight = true;
        _playerLastKnownPosition = playerPosition;
    }

    private void TargetOnLineOfSight (Vector3 playerPosition) 
    {
        _playerLastKnownPosition = playerPosition;
    }

    private void OnDisable() 
    {
        flashlight.onEnemyHitByLightAction -= HandleLightHit;
        flashlight.onEnemyLeftLightAction -= HandleLeftLight;
        fov.onTargetWithinRangeAction -= TargetOnLineOfSight;
    }

    void Start()
    {
        ConstructBehaviourTree();
        SetIdleMaterial();
        InitializeWaypointStack();
        StartCoroutine(fov.CheckWithDelay());
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0) {
            _topNode.Evaluate();
            if (_topNode.nodeState == Node.NodeState.FAILURE) 
            {
                SetIdleMaterial();
            }   
        }
    }

    public void RemovePositionFromStack()
    {
        if (_targets.Count > 0) _targets.Pop();
    }

    public void AddPositionToStack(Vector3 position)
    {
        _targets.Push(position);
    }

    public void Move()
    {
        agent.SetDestination(_targets.Peek());
    }

    public void RotateWithMovement()
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

    private void ConstructBehaviourTree()
    {
        IsCoverAvailableNode coverAvailableNode = new IsCoverAvailableNode(this);
        GoToCoverNode goToCoverNode = new GoToCoverNode(this);
        IsHitByLightNode hitByLightNode = new IsHitByLightNode(this);
        IsCoveredNode coveredNode = new IsCoveredNode(this);
        ChaseNode chaseNode = new ChaseNode(this);
        RangeNode chasingRangeNode = new RangeNode(this);
        IsWaypointAvailableNode waypointAvailableNode = new IsWaypointAvailableNode(this);
        PatrolNode patrolNode = new PatrolNode(this);

        Sequence chaseSequence = new Sequence (new List<Node> 
                {chasingRangeNode, chaseNode});
        Sequence patrolSequence = new Sequence (new List<Node>
                {waypointAvailableNode, patrolNode});
        Sequence goToCoverSequence = new Sequence (new List<Node> 
                {coverAvailableNode, goToCoverNode});
        Selector findCoverSelector = new Selector (new List<Node> 
                {goToCoverSequence, patrolSequence});
        Selector tryToTakeCoverSelector = new Selector (new List<Node> 
                {coveredNode, findCoverSelector});
        Sequence coverSequence = new Sequence (new List<Node>
                {hitByLightNode, tryToTakeCoverSelector});
        _topNode = new Selector (new List<Node>
                {coverSequence, chaseSequence, patrolSequence});

    }
    public void SetIdleMaterial()
    {
        _mesh.material = idleMat;
    }

    public Stack<Vector3> GetTargets() {
        return _targets;
    }
    
    public bool CheckHitByLight() {
        return _hitByLight;
    }
    
    public Vector3 GetPlayerLastKnownPosition() {
        return _playerLastKnownPosition;
    }
    public void HidingInCover()
    {
        _mesh.material = idleMat;
        _hitByLight = false;
    }

    internal void ChasingPlayer()
    {
        _mesh.material = chasingMat;
    }
    
    public int FindPossibleCovers()
    {
        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i] = null;
        }
        int hits = Physics.OverlapSphereNonAlloc(transform.position, coverRadius, _colliders, fov.obstructionMask);

        int hitReduction = 0;

        for (int i = 0; i < hits; i++)
        {
            if (Vector3.Distance(_colliders[i].transform.position, _playerLastKnownPosition) < minPlayerDistanceToCollider)
            {
                _colliders[i] = null;
                hitReduction++;
            }
        }

        hits -= hitReduction;

        System.Array.Sort(_colliders, ColliderArraySortComparer);

        return hits;
    }
    public Vector3 ProcessPossibleCovers(int hits)
    {
        for (int i = 0; i < hits; i++)
        {
            if (NavMesh.SamplePosition(_colliders[i].transform.position, out NavMeshHit hit, 2f, agent.areaMask))
            {
                if (!NavMesh.FindClosestEdge(hit.position, out hit, agent.areaMask))
                {
                    Debug.Log("No edge found close to " + hit.position);
                }
                if (Vector3.Dot(hit.normal, (_playerLastKnownPosition - hit.position).normalized) < hideSensitivity)
                {
                    return hit.position;
                }
            }
            else
            {
                if (NavMesh.SamplePosition(_colliders[i].transform.position - (_playerLastKnownPosition = hit.position).normalized * 2, out NavMeshHit newHit, 2f, agent.areaMask))
                {
                    if (!NavMesh.FindClosestEdge(newHit.position, out newHit, agent.areaMask))
                    {
                        Debug.Log("No edge found close to " + hit.position);
                    }
                    if (Vector3.Dot(newHit.normal, (_playerLastKnownPosition - newHit.position).normalized) < hideSensitivity)
                    {
                        return hit.position;
                    }
                }
            }
        }
        return Vector3.zero;
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
    private void InitializeWaypointStack()
    {
        if (patrolWaypoints.Count > 0) 
        {   
            patrolWaypoints.Reverse();
            foreach (Transform waypoint in patrolWaypoints)
            {
                _targets.Push(waypoint.position);
            }
        }
    }

}
