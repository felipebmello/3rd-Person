using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsHitByLightNode : Node
{

    private EnemyAITree ai;

    public IsHitByLightNode(EnemyAITree ai)
    {
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        if (ai.CheckHitByLight()) return NodeState.SUCCESS;
        else return NodeState.FAILURE;
    }
}
