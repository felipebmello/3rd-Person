using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [field: Range(1, 2)]
    [field: SerializeField] private int typeOfEnemy = 2;
    [field: SerializeField] protected Flashlight flashlight { get; private set; }
    [field: SerializeField] Material chasingMat;
    [field: SerializeField] Material idleMat;
    [field: SerializeField] Material blindMat;
    //Not being used, behaviour wasn't working properly
    [field: SerializeField] float smoothRotationTime = 0.2f;
    [field: SerializeField] public float coverRadius = 20f;
    [field: Range(-1, 1)]
    [field: SerializeField] public float hideSensitivity = -0.5f;
    [field: SerializeField] public float minPlayerDistanceToCollider = 1f;
    public NavMeshAgent agent { get; private set; }
    public  FieldOfView fov { get; private set; }
    public Collider[] _colliders = new Collider[10];
    private List<Transform> patrolWaypoints = new List<Transform>();
    private Stack<Vector3> _targets;
    private Vector3 _playerLastKnownPosition;
    private Node _topNode;
    private bool _hitByLight = false;
    private bool _hasSeenPlayer = false;
    private bool _movingTowardsCover = false;
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
        //_hitByLight = false;
    }

    private void HandleLightHit(Vector3 playerPosition, Collider collider)
    {
        if (collider.transform.parent.gameObject.Equals(gameObject)) {
            _hitByLight = true;
            _playerLastKnownPosition = playerPosition;
        }
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
                if (typeOfEnemy == 1) SetBlindMaterial();
                if (typeOfEnemy == 2) SetIdleMaterial();
            }
        }
    }

    private void SetBlindMaterial()
    {
        _mesh.material = blindMat;
    }

    public void RemovePositionFromStack()
    {
        if (_targets.Count > 0) {
            Debug.Log("Remove "+_targets.Peek()+" from the stack.");
            _targets.Pop();
        }
        else InitializeWaypointStack();
    }

    public void AddPositionToStack(Vector3 position)
    {
        Debug.Log("Add "+position+" to the stack.");
        _targets.Push(position);
    }

    internal bool HasSeenPlayer()
    {
        return _hasSeenPlayer;
    }

    public void Move()
    {
        agent.SetDestination(_targets.Peek());
    }

    public void Move(Vector3 target)
    {
        agent.SetDestination(target);
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
        /*Patrol behaviour was removed because enemies wouldn't stay behind cover.
        Another long-term solution would be to add another level of behaviour to 
        check if the player is still on the enemies line of sight.*/
        IsCoveredNode coveredNode = new IsCoveredNode(this, flashlight.transform.parent);
        IsHitByLightNode hitByLightNode = new IsHitByLightNode(this);
        IsCoverAvailableNode coverAvailableNode = new IsCoverAvailableNode(this);
        RangeNode chasingRangeNode = new RangeNode(this);
        IsLightOnNode lightOnNode = new IsLightOnNode(this);
        //IsWaypointAvailableNode waypointAvailableNode = new IsWaypointAvailableNode(this);
        //These three behaviours are implemented on top of a generic move node
        ChaseNode chaseNode = new ChaseNode(this);
        //PatrolNode patrolNode = new PatrolNode(this);
        GoToCoverNode goToCoverNode = new GoToCoverNode(this);
        
        Sequence chaseSequence = new Sequence (new List<Node> 
                {chasingRangeNode, chaseNode});
        /*Sequence patrolSequence = new Sequence (new List<Node>
                {waypointAvailableNode, patrolNode});*/
        Sequence goToCoverSequence = new Sequence (new List<Node> 
                {coverAvailableNode, goToCoverNode});
        Selector findCoverSelector = new Selector (new List<Node> 
                {goToCoverSequence});
        Selector tryToTakeCoverSelector = new Selector (new List<Node> 
                {coveredNode, findCoverSelector});
        Sequence coverSequence = new Sequence (new List<Node>
                {hitByLightNode, tryToTakeCoverSelector});

        
        Inverter lightOffNode = new Inverter (lightOnNode);
        Selector chaseLightOnSelector = new Selector (new List<Node> {lightOffNode, chaseNode});
        Sequence chaseLightSequence = new Sequence (new List<Node> {hitByLightNode, chaseLightOnSelector});
            
        //Doesn't have a field of vision, runs towards the player when the flashlight is on
        if (typeOfEnemy == 1)
        {_topNode = new Selector (new List<Node> {chaseLightSequence});
        }
        //Enemy type two is the enemy that has a field of view and runs from the flashlight
        else
        {
            _topNode = new Selector (new List<Node>
                {coverSequence, chaseSequence});
        }

    }
    
    public void SetIdleMaterial()
    {
        //Debug.Log("Change material to green");
        _mesh.material = idleMat;
    }

    public Stack<Vector3> GetTargets() {
        return _targets;
    }
    
    public bool CheckHitByLight() {
        return _hitByLight;
    }
    
    public bool CheckLightOn() {
        if (!flashlight.IsOn) _hitByLight = false;
        return flashlight.IsOn;
    }

    public Vector3 GetPlayerLastKnownPosition() {
        return _playerLastKnownPosition;
    }
    public void HidingInCover()
    {
        _hitByLight = false;
    }

    public void ChasingPlayer()
    {
        Debug.Log("Change material to red");
        _mesh.material = chasingMat;
    }

    internal void SetPlayerAsSeen(bool seen)
    {
        _hasSeenPlayer = seen;
    }
    internal bool IsMovingTowardsCover()
    {
        return _movingTowardsCover;
    }

    internal void SetMovingTowardsCover(bool moving)
    {
        _movingTowardsCover = moving;
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
    public void InitializeWaypointStack()
    {
        if (_targets.Count == 0 && patrolWaypoints.Count > 0) 
        {   
            patrolWaypoints.Reverse();
            foreach (Transform waypoint in patrolWaypoints)
            {
                _targets.Push(waypoint.position);
            }
        }
    }

}
