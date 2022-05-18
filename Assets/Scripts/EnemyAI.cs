using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] List<Transform> patrolWaypoints;
    
    [SerializeField] bool isPlayerInSightRange;
    [SerializeField] bool chaseLight = true;
    [SerializeField] float sightRange;
    // Parameter used on the SmoothDampAngle() function to store current velocity during each call
    [SerializeField] float smoothRotationTime = 0.2f;
    
    private Vector3 _target;
    private NavMeshAgent _agent;
    private FieldOfView _fov;
    private float _smothRotationVelocity;
    private int _patrolWaypointIndex;

    void Awake() 
    {
        // Stores reference to the NavMeshAgent component on the Enemy game object (this)
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _fov = gameObject.GetComponent<FieldOfView>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _patrolWaypointIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {

        EnemyMovement();
        
        
    }

    private void EnemyMovement()
    {

        UpdateDestination();
        RotateWithMovement();
        if (Vector3.Distance(gameObject.transform.position, _target) < 1f)
        {
            NextWaypoint();
        }
    }
    private void UpdateDestination()
    {
        _target = patrolWaypoints[_patrolWaypointIndex].position;
        _agent.SetDestination(_target);
        
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
        _patrolWaypointIndex++;
        if (_patrolWaypointIndex == patrolWaypoints.Count) 
        {
            _patrolWaypointIndex = 0;
            patrolWaypoints.Reverse();
        }
    }
}
