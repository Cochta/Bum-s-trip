using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Node;

public class Map : MonoBehaviour
{
    public enum NodeTypes
    {
        Start,
        Enemy,
        Shop,
        Treasure,
        Event,
        Boss
    }

    [SerializeField] private Sprite _startSprite;

    public List<Node> Nodes;

    private void Start()
    {
        Nodes = new List<Node>();
        //Nodes.Add(item: new Node(NodeTypes.Start, 0));
        //Nodes.Add(new Node(NodeTypes.Start, 1));
        //Nodes.Add(new Node(NodeTypes.Start, 1));
        //Nodes.Add(new Node(NodeTypes.Start, 2));
        //Nodes.Add(new Node(NodeTypes.Start, 2));
        //Nodes.Add(new Node(NodeTypes.Start, 3));
        //Nodes.Add(new Node(NodeTypes.Start, 4));
        //Nodes.Add(new Node(NodeTypes.Start, 4));
        //Nodes.Add(new Node(NodeTypes.Boss, 5));
    }
}
