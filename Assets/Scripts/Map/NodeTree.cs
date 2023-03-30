using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeTree : MonoBehaviour
{
    public Node Root;

    private int MaxDepth = 3;
    private int MaxNodesPerDepth = 3;

    List<int> _nodeNumbers = new List<int>() { 1, 3, 2, 1 };

    private void Start()
    {
        Root = new(Node.NodeTypes.Start, 0);
        Root.AddNode(0, MaxDepth, _nodeNumbers, 0, null);
    }
}
