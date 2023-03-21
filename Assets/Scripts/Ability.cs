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

        _PoolManager.Player.ActionPoints -= 1;
    }

    protected virtual void OnMouseEnter()
    {
        Tile value;
        foreach (var target in Targets)
        {
            if (_PoolManager.Player.grid.TryGetValue(_PoolManager.Player.tile._position + target, out value))
            {
                _PoolManager.Player.grid[_PoolManager.Player.tile._position + target].HighLight(HighlightColor);
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
            foreach (var tile in _PoolManager.Player.grid)
            {
                tile.Value.CanceHighlight();
            }
        }
    }
}
