
using UnityEngine;
using UnityEngine.AI;

public class ChaseNode : MoveNode
{

    public ChaseNode(EnemyAITree ai) : base (ai)
    {
    }

    public override NodeState Evaluate()
    {
        target = ai.GetPlayerLastKnownPosition();
        ai.AddPositionToStack(target);
        return base.Evaluate();
    }
}
