using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [field: Header("Field of View Values")]
    [field: SerializeField] public float viewRadius { get; protected set; }
    [field: Range(0, 360)]
    [field: SerializeField]  public float viewAngle { get; protected set; }
    [field: SerializeField] public bool canSeePlayer { get; protected set; }


    [field: Header("Layer Masks")]
    [field: SerializeField] public LayerMask targetMask;
    [field: SerializeField] public LayerMask obstructionMask;
    
    private Transform _target;
    public event Action<Vector3> onTargetWithinRangeAction;
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckWithDelay());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator CheckWithDelay()
    {
        float delay = 0.2f;
        while (true) 
        {
            yield return new WaitForSeconds (delay);
            CheckFOV();
        }
    }
    public Vector3 DirectionFromAngle(float angleInDegrees, bool isGlobal) {
        if (!isGlobal) angleInDegrees += transform.eulerAngles.y;
        return new Vector3 (Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void CheckFOV()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        if (rangeChecks.Length != 0) 
        {
            _target = rangeChecks[0].transform;

            Vector3 directionToTarget = (_target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle /2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, _target.position);
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask)) 
                {
                    canSeePlayer = true;
                }
                else canSeePlayer = false;
            }
            else canSeePlayer = false;
        }
        else if (canSeePlayer) canSeePlayer = false;

    }

    public Vector3 GetLastKnownTargetPosition() 
    {
        return _target.position;
    }
}
