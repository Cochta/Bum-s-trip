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
    private int _maxDepth = 5;
    private int _maxNodesPerDepth = 4;
    private int _minNodesPerDepth = 2;

    [SerializeField] private GameObject NodesLocationMap;

    public GameObject NodePrefab;
    public GameObject ArrowPrefab;

    private List<Node> _nodes = new List<Node>();
    private List<MapArrow> _arrows;

    private System.Random _rnd = new System.Random();

    public void GenerateMap()
    {
        List<PlayerData.GameStates> specialNodes = new List<PlayerData.GameStates>() { PlayerData.GameStates.ToTreasure, PlayerData.GameStates.ToEvent, PlayerData.GameStates.ToShop };
        int nonAvaillableDeth = _rnd.Next(1, _maxDepth);

        // Add start node
        AddNode(PlayerData.GameStates.None, 0);

        for (int i = 1; i < _maxDepth; i++)
        {
            int nbNodes = _rnd.Next(_minNodesPerDepth, _maxNodesPerDepth + 1);
            int nonEnemyNode = _rnd.Next(0, nbNodes); // choose a random node to not be an enemy

            for (int j = 0; j < nbNodes; j++)
            {
                PlayerData.GameStates type = PlayerData.GameStates.ToBattle;
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
        AddNode(PlayerData.GameStates.ToBoss, _maxDepth);

        LinkNodes();

        DisplayNodes();

        GenerateArrows();

        SetPath();
    }
    private void AddNode(PlayerData.GameStates type, int depth)
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
        for (int i = 0; i < _maxDepth; i++)
        {
            List<Node> nodesOnThisDepth = new List<Node>();
            foreach (var node in _nodes)
                if (node.Depth == i) nodesOnThisDepth.Add(node);

            List<Node> nodesOnUpperDepth = new List<Node>();
            foreach (var node in _nodes)
                if (node.Depth == i + 1) nodesOnUpperDepth.Add(node);

            int nbThisNodes = nodesOnThisDepth.Count;
            int nbUpNodes = nodesOnUpperDepth.Count;

            if (nbThisNodes == 1)
            {
                foreach (Node node in nodesOnUpperDepth)
                {
                    nodesOnThisDepth[0].AccessibleNodes.Add(node);
                }
            }
            else if (nbUpNodes == 1)
            {
                foreach (Node node in nodesOnThisDepth)
                {
                    node.AccessibleNodes.Add(nodesOnUpperDepth[0]);
                }
            }
            else if (nbThisNodes == nbUpNodes)
            {
                for (int j = 0; j < nbThisNodes; j++)
                {
                    nodesOnThisDepth[j].AccessibleNodes.Add(nodesOnUpperDepth[j]);
                }
            }
            else if (nbThisNodes + 1 == nbUpNodes) // 2 to 3 && 3 to 4
            {
                for (int j = 0; j < nbThisNodes; j++)
                {
                    nodesOnThisDepth[j].AccessibleNodes.Add(nodesOnUpperDepth[j]);
                    nodesOnThisDepth[j].AccessibleNodes.Add(nodesOnUpperDepth[j + 1]);
                }

            }
            else if (nbThisNodes == nbUpNodes + 1) // 3 to 2 && 4 to 3
            {
                for (int j = 0; j < nbThisNodes; j++)
                {
                    if (j > 0)
                        nodesOnThisDepth[j].AccessibleNodes.Add(nodesOnUpperDepth[j - 1]);
                    if (j < nbUpNodes)
                        nodesOnThisDepth[j].AccessibleNodes.Add(nodesOnUpperDepth[j]);
                }

            }
            else if (nbThisNodes + 2 == nbUpNodes) // 2 to 4
            {
                int k = 0;
                for (int j = 0; j < nbThisNodes; j++)
                {
                    nodesOnThisDepth[j].AccessibleNodes.Add(nodesOnUpperDepth[k]);
                    nodesOnThisDepth[j].AccessibleNodes.Add(nodesOnUpperDepth[k + 1]);
                    k += 2;
                }
            }
            else if (nbThisNodes == nbUpNodes + 2) // 4 to 2
            {
                int k = 0;
                for (int j = 0; j < nbThisNodes; j++)
                {
                    if (j % 2 == 0 && j > 0)
                        k += 1;
                    nodesOnThisDepth[j].AccessibleNodes.Add(nodesOnUpperDepth[k]);
                }
            }
        }
    }
    private void DisplayNodes()
    {
        var BasePadding = 0.6f;
        var PaddingIncrement = 0.3f;

        for (int i = 0; i <= _maxDepth; i++)
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
                node.Arrows.Add(arrow);
                _arrows.Add(arrow);
            }
        }
    }
    public void SetPath()
    {
        foreach (var arrow in _arrows)
        {
            arrow.SetDefaultColor();
        }
        foreach (var arrow in PlayerData.Instance.Node.Arrows)
        {
            arrow.SetColor(Color.white);
        }
        foreach (var node in _nodes)
        {
            node.Enabled(false);
        }
        foreach (var child in PlayerData.Instance.Node.AccessibleNodes)
        {
            child.Enabled(true);
        }
    }
}
