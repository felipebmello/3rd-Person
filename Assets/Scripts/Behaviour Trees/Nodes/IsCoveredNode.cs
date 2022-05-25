using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCoveredNode : Node
{

    private Transform targetPlayer;
    private EnemyAI ai;

    public IsCoveredNode(EnemyAI ai, Transform targetPlayer)
    {
        this.ai = ai;
        this.targetPlayer = targetPlayer;
    }

    public override NodeState Evaluate()
    {
        Debug.Log(this.GetType());
        ai.SetIdleMaterial();
        RaycastHit hit;
        if (Physics.Raycast(ai.transform.position, (targetPlayer.position - ai.transform.position), out hit))
        {
            Debug.DrawLine (ai.transform.position, targetPlayer.position );
            if (!hit.collider.transform.CompareTag("Player")) 
            {
                //add a timer? or maybe make the light permanently keep the enemy hidden.
                ai.HidingInCover();
                return NodeState.SUCCESS;
            }
        }
        return NodeState.FAILURE;
    }
}
