
using UnityEngine;
using UnityEngine.AI;

public class GoToCoverNode : MoveNode
{
    public GoToCoverNode(EnemyAITree ai) : base (ai)
    {
    }

    public override NodeState Evaluate()
    {
        target = ai.GetHidingSpot();
        if (target == Vector3.zero) return NodeState.FAILURE;
        ai.AddPositionToStack(target);
        return base.Evaluate();
    }
}
