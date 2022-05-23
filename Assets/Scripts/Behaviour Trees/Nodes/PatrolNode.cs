using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolNode : MoveNode
{

    public PatrolNode (EnemyAITree ai) : base (ai)
    {
    }

    public override NodeState Evaluate()
    {
        return base.Evaluate();
    }
}
