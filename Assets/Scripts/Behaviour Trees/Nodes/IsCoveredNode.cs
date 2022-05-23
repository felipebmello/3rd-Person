using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCoveredNode : Node
{

    private Vector3 targetPosition;
    private EnemyAITree ai;

    public IsCoveredNode(EnemyAITree ai)
    {
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        targetPosition = ai.GetPlayerLastKnownPosition();
        RaycastHit hit;
        if (Physics.Raycast(ai.transform.position, targetPosition - ai.transform.position, out hit))
        {
            if (!hit.collider.transform.CompareTag("Player")) 
            {
                ai.HidingInCover();
                return NodeState.SUCCESS;
            }
        }
        return NodeState.FAILURE;
    }
}
