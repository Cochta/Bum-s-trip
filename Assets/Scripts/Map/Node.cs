using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private NodeTypes _type;
    private int _depth;
    public List<Node> AccessibleNodes = new List<Node>();
    public List<MapArrow> Arrows = new List<MapArrow>();

    [SerializeField] private Sprite _spriteStart;
    [SerializeField] private Sprite _spriteEnemy;
    [SerializeField] private Sprite _spriteShop;
    [SerializeField] private Sprite _spriteTreasure;
    [SerializeField] private Sprite _spriteEvent;
    [SerializeField] private Sprite _spriteBoss;

    [SerializeField] private SpriteRenderer _sR;
    [SerializeField] private SpriteRenderer _sRActive;
    [SerializeField] private SpriteRenderer _sRBackground;
    [SerializeField] private BoxCollider2D _col;

    public NodeTypes Type { get => _type; set => _type = value; }
    public int Depth { get => _depth; set => _depth = value; }

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

    public void Enabled(bool enabled)
    {
        _sRActive.enabled = enabled;
        _col.enabled = enabled;
        _sRBackground.enabled = false;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayerData.Instance.Node = this;
            if (Type == NodeTypes.Enemy)
                PlayerData.Instance.ChangeGameState(PlayerData.GameStates.ToBattle);
            else if (Type == NodeTypes.Shop)
                PlayerData.Instance.ChangeGameState(PlayerData.GameStates.ToShop);
            else if (Type == NodeTypes.Treasure)
                PlayerData.Instance.ChangeGameState(PlayerData.GameStates.ToTreasure);
            else if (Type == NodeTypes.Event)
                PlayerData.Instance.ChangeGameState(PlayerData.GameStates.ToEvent);
            else if (Type == NodeTypes.Boss)
                PlayerData.Instance.ChangeGameState(PlayerData.GameStates.ToBoss);

        }
    }

    private void OnMouseEnter()
    {
        _sRBackground.enabled = true;
    }

    private void OnMouseExit()
    {
        _sRBackground.enabled = false;
    }
}
