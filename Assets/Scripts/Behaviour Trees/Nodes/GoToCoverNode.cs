
using UnityEngine;
using UnityEngine.AI;

public class GoToCoverNode : MoveNode
{
    public GoToCoverNode(EnemyAI ai) : base (ai)
    {
    }

    public override NodeState Evaluate()
    {
        Debug.Log(this.GetType()+" "+ai.GetTargets().Count);
        target = ai.GetHidingSpot();
        if (target == Vector3.zero) return NodeState.FAILURE;
        return base.Evaluate();
    }
}
