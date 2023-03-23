using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public MovePoolManager _PoolManager;
    public List<Vector2> Targets;

    public bool IsSelected = false;

    public Color HighlightColor = Color.green;
    public BoxCollider2D _col;

    protected virtual void Awake()
    {
        _PoolManager = GetComponentInParent<MovePoolManager>();
        _col = GetComponent<BoxCollider2D>();
    }

    public virtual void PerformAction(Tile tile)
    {
        IsSelected = false;
        tile.GetComponentInParent<Grid>().CancelHighlight();

        PlayerData.Instance.ActionPoints -= 1;
    }

    protected virtual void OnMouseEnter()
    {
        Tile value;
        foreach (var target in Targets)
        {
            if (_PoolManager.Player.grid._tiles.ContainsKey(_PoolManager.Player.tile._position + target))
            {
                _PoolManager.Player.grid._tiles[_PoolManager.Player.tile._position + target].HighLight(HighlightColor);
            }
        }
    }

    protected virtual void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (var ability in _PoolManager.Abilities)
            {
                ability.IsSelected = false;
            }
            IsSelected = true;
        }
    }

    protected virtual void OnMouseExit()
    {
        if (!IsSelected)
        {
            foreach (var tile in _PoolManager.Player.grid._tiles)
            {
                tile.Value.CanceHighlight();
            }
        }
    }
}
