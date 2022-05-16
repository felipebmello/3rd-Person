using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] List<Transform> waypoints;
    private NavMeshAgent _agent;
    //private LayerMask _isGround, _isPlayer;
    [SerializeField] float smoothRotationTime = 0.2f;
    // Parameter used on the SmoothDampAngle() function to store current velocity during each call
    
    private Vector3 _target;
    private float _smothRotationVelocity;
    private int _waypointIndex;

    void Awake() 
    {
        // Stores reference to the NavMeshAgent component on the Enemy game object (this)
        _agent = gameObject.GetComponent<NavMeshAgent>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _waypointIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
        if (Vector3.Distance(gameObject.transform.position, _target) < 1f)
        {
            NextWaypoint();
        }
    }

    private void EnemyMovement()
    {

        UpdateDestination();
        RotateWithMovement();
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

    private void UpdateDestination()
    {
        _target = waypoints[_waypointIndex].position;
        _agent.SetDestination(_target);
        
    }

    private void NextWaypoint() 
    {
        _waypointIndex++;
        if (_waypointIndex == waypoints.Count) 
        {
            _waypointIndex = 0;
            waypoints.Reverse();
        }
    }
}
