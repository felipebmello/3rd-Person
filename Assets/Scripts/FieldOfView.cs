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
    [field: Range(0, 1)]
    [field: SerializeField]  public float delay { get; protected set; }


    [field: Header("Layer Masks")]
    [field: SerializeField] public LayerMask targetMask;
    [field: SerializeField] public LayerMask obstructionMask;

    private Transform _target;
    private bool _canSeeTarget = false;
    private FieldOfViewServices fovServices;
    public event Action<Vector3> onTargetWithinRangeAction;
    

    // Start is called before the first frame update
    void Start()
    {
        fovServices = new FieldOfViewServices(viewRadius, viewAngle, targetMask, obstructionMask);
        
    }

    public IEnumerator CheckWithDelay()
    {
        while (true) 
        {
            yield return new WaitForSeconds (delay);
            _canSeeTarget = fovServices.CheckFOV(transform.position, transform.forward);
            if (_canSeeTarget) 
            {
                _target = fovServices.GetTarget();
                onTargetWithinRangeAction.Invoke(_target.position);
            }
        }
    }
    public Vector3 DirectionFromAngle(float angleInDegrees, bool isGlobal) {
        if (!isGlobal) angleInDegrees += transform.eulerAngles.y;
        return new Vector3 (Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }


    public Vector3 GetLastKnownTargetPosition() 
    {
        return _target.position;
    }
    public bool CanSeeTarget () {
        return _canSeeTarget;
    }
}
