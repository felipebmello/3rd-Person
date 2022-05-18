using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVServices
{
    public float viewRadius { get; protected set; }
    public float viewAngle { get; protected set; }
    public LayerMask targetMask { get; protected set; }
    public LayerMask obstructionMask { get; protected set; }

    private Transform _target;
    private bool _canSeeTarget;

    public FOVServices(float viewRadius, float viewAngle, LayerMask targetMask, LayerMask obstructionMask)
    {
        this.viewRadius = viewRadius;
        this.viewAngle = viewAngle;
        this.targetMask = targetMask;
        this.obstructionMask = obstructionMask;
    } 

    public bool CheckFOV (Vector3 centerPosition, Vector3 forward)
    {
        Collider[] rangeChecks = Physics.OverlapSphere(centerPosition, viewRadius, targetMask);
        if (rangeChecks.Length != 0) 
        {
            _target = rangeChecks[0].transform;

            Vector3 directionToTarget = (_target.position - centerPosition).normalized;
            if (Vector3.Angle(forward, directionToTarget) < viewAngle /2)
            {
                float distanceToTarget = Vector3.Distance(centerPosition, _target.position);
                if (!Physics.Raycast(centerPosition, directionToTarget, distanceToTarget, obstructionMask)) 
                {
                    _canSeeTarget = true;
                }
                else _canSeeTarget = false;
            }
            else _canSeeTarget = false;
        }
        else if (_canSeeTarget) _canSeeTarget = false;
        return _canSeeTarget;
    }

    public Transform GetTarget() 
    {
        return _target;
    }

}
