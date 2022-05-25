using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : Node
{
    
    private EnemyAI ai;


    public RangeNode(EnemyAI ai)
    {
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        Debug.Log(this.GetType()+" returns "+ai.fov.CanSeeTarget());
        if (ai.fov.CanSeeTarget()) 
        {
            return NodeState.SUCCESS;
        }
        else 
        {
            return NodeState.FAILURE;
        }
    }
}
