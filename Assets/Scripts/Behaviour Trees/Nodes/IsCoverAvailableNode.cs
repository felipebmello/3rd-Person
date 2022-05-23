using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IsCoverAvailableNode : Node
{
    private EnemyAITree ai;

    public IsCoverAvailableNode(EnemyAITree ai)
    {
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        Vector3 hidingSpot = FindCover();
        ai.SetHidingSpot(hidingSpot);
        if (hidingSpot != Vector3.zero) 
        {
            return NodeState.SUCCESS;
        }
        else return NodeState.FAILURE;
    }

    private Vector3 FindCover()
    {
        int hits = ai.FindPossibleCovers();
        return ai.ProcessPossibleCovers(hits);
    }

    
}
