using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolNode : MoveNode
{

    public PatrolNode (EnemyAI ai) : base (ai)
    {
    }

    public override NodeState Evaluate()
    {
        Debug.Log(this.GetType());
        target = ai.GetTargets().Pop();
        return base.Evaluate();
    }
}
