using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Node : MonoBehaviour
{
    private PlayerData.GameStates _type;
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

    public PlayerData.GameStates Type { get => _type; set => _type = value; }
    public int Depth { get; set; }

    public void DefineSprite()
    {
        if (Type == PlayerData.GameStates.ToBoss)
            _sR.sprite = _spriteBoss;
        else if (Type == PlayerData.GameStates.ToBattle)
            _sR.sprite = _spriteEnemy;
        else if (Type == PlayerData.GameStates.ToEvent)
            _sR.sprite = _spriteEvent;
        else if (Type == PlayerData.GameStates.ToShop)
            _sR.sprite = _spriteShop;
        else if (Type == PlayerData.GameStates.ToTreasure)
            _sR.sprite = _spriteTreasure;
        else
            _sR.sprite = _spriteStart;
    }

    public void Enabled(bool enabled)
    {
        _sRActive.enabled = enabled;
        _col.enabled = enabled;
        _sRBackground.enabled = false;
    }

    private void OnMouseDown()
    {
        PlayerData.Instance.Node = this;
        PlayerData.Instance.ChangeGameState(this.Type);
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
