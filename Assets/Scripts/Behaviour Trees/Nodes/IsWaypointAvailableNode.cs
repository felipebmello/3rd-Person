using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsWaypointAvailableNode : Node
{
    private EnemyAI ai;

    public IsWaypointAvailableNode(EnemyAI ai)
    {
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        Debug.Log(this.GetType()+" returns ");
        if (ai.GetTargets().Count > 0) {
            ai.SetIdleMaterial();
            return NodeState.SUCCESS;
        }
        ai.InitializeWaypointStack();
        return NodeState.FAILURE;
    }
}

