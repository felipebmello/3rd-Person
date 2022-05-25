using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsLightOnNode : Node
{

    private EnemyAI ai;

    public IsLightOnNode(EnemyAI ai)
    {
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        Debug.Log(this.GetType()+" returns "+ai.CheckHitByLight());
        if (ai.CheckLightOn()) return NodeState.SUCCESS;
        else 
        {
            return NodeState.FAILURE;
        }
    }
}
