using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Map.NodeTypes Type;
    public int Depth;

    public Node(Map.NodeTypes type, int depth)
    {
        Depth = depth;
        Type = type;
    }
}
