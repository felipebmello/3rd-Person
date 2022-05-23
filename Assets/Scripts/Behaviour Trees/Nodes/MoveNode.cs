using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNode : Node
{
    protected Vector3 target;
    protected EnemyAITree ai;

    public MoveNode(EnemyAITree ai)
    {
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        if (Vector3.Distance(target, ai.transform.position) > 0.2f) {
            ai.Move();
            ai.RotateWithMovement();
            return NodeState.RUNNING;
        }
        else
        {
            ai.RemovePositionFromStack();
            return NodeState.SUCCESS;
        }
    }
}
