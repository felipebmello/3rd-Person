using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : Node
{
    
    private EnemyAITree ai;


    public RangeNode(EnemyAITree ai)
    {
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        if (ai.fov.CanSeeTarget()) 
        {
            ai.AddPositionToStack(ai.transform.position);
            ai.ChasingPlayer();
            return NodeState.SUCCESS;
        }
        else return NodeState.FAILURE;
    }
}
