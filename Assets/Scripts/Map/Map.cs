using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Node;

public class Map : MonoBehaviour
{
    private int MaxDepth = 5;
    private int MaxNodesPerDepth = 3;

    [SerializeField] private GameObject NodesLocationMap;

    public GameObject NodePrefab;
    public GameObject ArrowPrefab;

    private List<Node> _nodes = new List<Node>();
    private List<MapArrow> _arrows;

    private System.Random _rnd = new System.Random();

    NodeTree _tree = new NodeTree();
    private void Start()
    {
        GenerateMap();
    }

    private void OnDrawGizmos()
    {
        List<Color> colors = new List<Color>() { Color.red, Color.blue, Color.green, Color.magenta, Color.yellow, Color.white, Color.black };
        int i = 0;
        foreach (var node in _nodes)
        {
            Gizmos.color = colors[i++];
            if (i > colors.Count - 1) i = 0;
            foreach (var parent in node.AccessibleNodes)
            {

                Gizmos.DrawLine(node.transform.position, parent.transform.position);
            }
        }
    }

    private void GenerateMap()
    {
        List<Node.NodeTypes> specialNodes = new List<Node.NodeTypes>() { Node.NodeTypes.Treasure, Node.NodeTypes.Event, Node.NodeTypes.Shop };
        int nonAvaillableDeth = _rnd.Next(1, MaxDepth);

        // Add start node
        AddNode(Node.NodeTypes.Start, 0);

        for (int i = 1; i < MaxDepth; i++)
        {
            int nbNodes = _rnd.Next(1, MaxNodesPerDepth + 1);
            int nonEnemyNode = _rnd.Next(0, nbNodes); // choose a random node to not be an enemy

            for (int j = 0; j < nbNodes; j++)
            {
                Node.NodeTypes type = Node.NodeTypes.Enemy;
                if (i != nonAvaillableDeth)
                {
                    if (j == nonEnemyNode)
                    {
                        var index = _rnd.Next(0, specialNodes.Count - 1);
                        type = specialNodes[index];
                        specialNodes.RemoveAt(index);
                    }
                }
                AddNode(type, i);
            }
        }
        // Add boss node
        AddNode(Node.NodeTypes.Boss, MaxDepth);

        LinkNodes();

        DisplayNodes();

        GenerateArrows();
        DisplayMap();
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
        if (depth == 0)
            PlayerData.Instance.Node = node;

        _nodes.Add(node);
    }
    private void LinkNodes()
    {
        for (int i = 0; i < MaxDepth; i++)
        {
            List<Node> nodesOnThisDepth = new List<Node>();
            foreach (var node in _nodes)
            {
                if (node.Depth == i) nodesOnThisDepth.Add(node);
            }

            List<Node> nodesOnUpperDepth = new List<Node>();
            foreach (var node in _nodes)
            {
                if (node.Depth == i + 1) nodesOnUpperDepth.Add(node);
            }

            if (nodesOnThisDepth.Count == 1)
            {
                foreach (Node node in nodesOnUpperDepth)
                {
                    nodesOnThisDepth[0].AccessibleNodes.Add(node);
                }
            }
            else if (nodesOnUpperDepth.Count == 1)
            {
                foreach (Node node in nodesOnThisDepth)
                {
                    node.AccessibleNodes.Add(nodesOnUpperDepth[0]);
                }
            }
            else if (nodesOnThisDepth.Count == 2 && nodesOnUpperDepth.Count == 2)
            {
                nodesOnThisDepth[0].AccessibleNodes.Add(nodesOnUpperDepth[0]);
                nodesOnThisDepth[1].AccessibleNodes.Add(nodesOnUpperDepth[1]);
            }
            else if (nodesOnThisDepth.Count == 2 && nodesOnUpperDepth.Count == 3)
            {
                nodesOnThisDepth[0].AccessibleNodes.Add(nodesOnUpperDepth[0]);
                nodesOnThisDepth[0].AccessibleNodes.Add(nodesOnUpperDepth[1]);
                nodesOnThisDepth[1].AccessibleNodes.Add(nodesOnUpperDepth[1]);
                nodesOnThisDepth[1].AccessibleNodes.Add(nodesOnUpperDepth[2]);
            }
            else if (nodesOnThisDepth.Count == 3 && nodesOnUpperDepth.Count == 2)
            {
                nodesOnThisDepth[0].AccessibleNodes.Add(nodesOnUpperDepth[0]);
                nodesOnThisDepth[1].AccessibleNodes.Add(nodesOnUpperDepth[0]);
                nodesOnThisDepth[1].AccessibleNodes.Add(nodesOnUpperDepth[1]);
                nodesOnThisDepth[2].AccessibleNodes.Add(nodesOnUpperDepth[1]);
            }
            else if (nodesOnThisDepth.Count == 3 && nodesOnUpperDepth.Count == 3)
            {
                nodesOnThisDepth[0].AccessibleNodes.Add(nodesOnUpperDepth[0]);
                nodesOnThisDepth[1].AccessibleNodes.Add(nodesOnUpperDepth[1]);
                nodesOnThisDepth[2].AccessibleNodes.Add(nodesOnUpperDepth[2]);
            }
        }
    }
    void DisplayNodes()
    {
        var BasePadding = 0.6f;
        var PaddingIncrement = 0.3f;

        for (int i = 0; i <= MaxDepth; i++)
        {
            var nodesInLevel = _nodes.Where(n => n.Depth == i).ToList();
            var levelWidth = nodesInLevel.Count * (BasePadding + PaddingIncrement) - PaddingIncrement;
            var offset = -levelWidth / 2f;

            foreach (Node node in nodesInLevel)
            {
                node.transform.localPosition = new Vector3(offset + PaddingIncrement / 2 + 0.5f * PaddingIncrement, i * BasePadding, 0);
                offset += BasePadding + PaddingIncrement;
            }
        }
    }
    private void GenerateArrows()
    {
        _arrows = new List<MapArrow>();
        foreach (var node in _nodes)
        {
            foreach (var parent in node.AccessibleNodes)
            {
                var arrow = Instantiate(ArrowPrefab, node.transform).GetComponent<MapArrow>();
                arrow.transform.localPosition = (parent.transform.localPosition - node.transform.localPosition) * 1.3f;
                arrow.transform.up = (parent.transform.localPosition - node.transform.localPosition).normalized;
                arrow.Node = node;
                _arrows.Add(arrow);
            }
        }
    }

    private void DisplayMap()
    {

        foreach (var arrow in _arrows)
        {
            if (arrow.Node == PlayerData.Instance.Node)
                arrow.SetColor(Color.white);
            else arrow.SetDefaultColor();
        }

    }
}
