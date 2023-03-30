using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
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

    public NodeTypes Type;
    public int Depth;
    public List<Node> Childs = new List<Node>();

    public List<Node> AccessibleNodes = new List<Node>();

    [SerializeField] private Sprite _spriteStart;
    [SerializeField] private Sprite _spriteEnemy;
    [SerializeField] private Sprite _spriteShop;
    [SerializeField] private Sprite _spriteTreasure;
    [SerializeField] private Sprite _spriteEvent;
    [SerializeField] private Sprite _spriteBoss;

    [SerializeField] private SpriteRenderer _sR;

    public Node(NodeTypes type, int depth)
    {
        Type = type;
        Depth = depth;
    }

    public void AddNode(int depth, int depthMax, List<int> nodeNumbers, int currentNodeNumber, Node Parent)
    {
        depth++;

        if (depth >= depthMax) return; // BossNode


        if (currentNodeNumber < nodeNumbers[depth])
        {
            Node newNode = new(NodeTypes.Enemy, depth);
            Childs.Add(newNode);
            currentNodeNumber++;
            newNode.AddNode(depth, depthMax, nodeNumbers, currentNodeNumber, this);
            Debug.Log(Depth + " | " + Type);
        }
    }

    public void DefineSprite()
    {
        if (Type == NodeTypes.Boss)
            _sR.sprite = _spriteBoss;
        else if (Type == NodeTypes.Enemy)
            _sR.sprite = _spriteEnemy;
        else if (Type == NodeTypes.Event)
            _sR.sprite = _spriteEvent;
        else if (Type == NodeTypes.Shop)
            _sR.sprite = _spriteShop;
        else if (Type == NodeTypes.Treasure)
            _sR.sprite = _spriteTreasure;
        else if (Type == NodeTypes.Start)
            _sR.sprite = _spriteStart;
    }


}
