using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsHitByLightNode : Node
{

    private EnemyAI ai;

    public IsHitByLightNode(EnemyAI ai)
    {
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        Debug.Log(this.GetType()+" returns "+ai.CheckHitByLight());
        if (ai.CheckHitByLight()) return NodeState.SUCCESS;
        else return NodeState.FAILURE;
    }
}
