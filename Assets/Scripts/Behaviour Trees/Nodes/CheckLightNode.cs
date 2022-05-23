using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLightNode : Node
{

    private EnemyAI _enemy;

    public CheckLightNode(EnemyAI enemy)
    {
        _enemy = enemy;
    }

    public override NodeState Evaluate()
    {
        throw new System.NotImplementedException();
    }
}
