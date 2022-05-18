using System.Collections.Generic;

public class Sequence : Node
{
    protected List<Node> _nodes = new List<Node>();
    public Sequence(List<Node> nodes)
    {
        _nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        bool isAnyNodeRunning = false;
        foreach (var node in _nodes) {
            switch (node.Evaluate())
            {
                case NodeState.RUNNING:
                    isAnyNodeRunning = true;
                    break;
                case NodeState.SUCCESS:
                    break;
                case NodeState.FAILURE:
                    _nodeState = NodeState.FAILURE;
                    return _nodeState;
                default:
                    break;
            }
        }
        if (isAnyNodeRunning) {
            _nodeState = NodeState.RUNNING;
        }
        else
        {
            _nodeState = NodeState.SUCCESS;
        }
        return _nodeState;
    }

}
