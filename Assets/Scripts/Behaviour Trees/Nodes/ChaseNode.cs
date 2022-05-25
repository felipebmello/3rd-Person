
using UnityEngine;
using UnityEngine.AI;

public class ChaseNode : MoveNode
{

    public ChaseNode(EnemyAI ai) : base (ai)
    {
    }

    public override NodeState Evaluate()
    {
        this.GetType();
        ai.ChasingPlayer();
        target = ai.GetPlayerLastKnownPosition();
        return base.Evaluate();
    }
}
