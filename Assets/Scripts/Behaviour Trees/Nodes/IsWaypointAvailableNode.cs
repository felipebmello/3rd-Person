using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsWaypointAvailableNode : Node
{
    private EnemyAITree ai;

    public IsWaypointAvailableNode(EnemyAITree ai)
    {
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        if (ai.GetTargets().Count > 0) {
            
            ai.SetIdleMaterial();
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
