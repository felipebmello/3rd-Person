using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNode : Node
{
    protected Vector3 target;
    protected EnemyAI ai;

    public MoveNode(EnemyAI ai)
    {
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        Debug.Log(this.GetType());
        if (Vector3.Distance(target, ai.transform.position) > 0.5f) {
            ai.Move(target);
            ai.RotateWithMovement();
            return NodeState.RUNNING;
        }
        else
        {
            return NodeState.SUCCESS;
        }
    }
}
