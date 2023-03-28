using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Node;

public class Map : MonoBehaviour
{
    public int MaxDepth;
    public int MaxNodesPerDepth;

    [SerializeField] private GameObject NodesLocationMap;

    public GameObject NodePrefab;

    private List<Node> _nodes = new List<Node>();

    private System.Random _rnd = new System.Random();

    private void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        // Add start node
        AddNode(Node.NodeTypes.Start, 0);

        for (int i = 1; i < MaxDepth; i++)
        {
            int nbNodes = _rnd.Next(1, MaxNodesPerDepth);
            int nonEnemyNode = _rnd.Next(0, nbNodes); // choose a random node to not be an enemy

            for (int j = 0; j < nbNodes; j++)
            {
                Node.NodeTypes type = Node.NodeTypes.Enemy;
                if (j == nonEnemyNode)
                {
                    // randomly choose a non-enemy node type
                    int nonEnemyType = _rnd.Next(0, 3);
                    switch (nonEnemyType)
                    {
                        case 0:
                            type = Node.NodeTypes.Treasure;
                            break;
                        case 1:
                            type = Node.NodeTypes.Shop;
                            break;
                        case 2:
                            type = Node.NodeTypes.Event;
                            break;
                    }
                }
                AddNode(type, i);
            }
        }


        // Add boss node
        AddNode(Node.NodeTypes.Boss, MaxDepth);

        foreach (var node in _nodes)
        {
            Debug.Log(node.Type + " | " + node.Depth);
        }
    }

    private void AddNode(Node.NodeTypes type, int depth)
    {
        // Instantiate node prefab and set type and depth
        GameObject nodeObj = Instantiate(NodePrefab, NodesLocationMap.transform);
        Node node = nodeObj.GetComponent<Node>();
        node.Type = type;
        node.Depth = depth;

        node.DefineSprite();

        // Add node to list of nodes
        _nodes.Add(node);

        // Position nodes at equal distances on x-axis
        int numNodesAtDepth = _nodes.Count(n => n.Depth == depth);
        float spacing = GetComponentInChildren<Renderer>().bounds.size.x / (numNodesAtDepth + 1);

        for (int i = 0; i < numNodesAtDepth; i++)
        {
            Node nodeAtDepth = _nodes.Where(n => n.Depth == depth).ElementAt(i);

            nodeAtDepth.transform.position = new Vector3(nodeAtDepth.transform.parent.position.x - 1.75f + spacing * (i + 1), nodeAtDepth.transform.parent.position.y + (float)depth / 2, 0);
        }
    }
}
