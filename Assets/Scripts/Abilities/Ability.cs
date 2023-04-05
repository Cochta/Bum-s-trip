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
    }

    protected virtual void OnMouseEnter()
    {
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
    protected IEnumerable<Vector2> GetNeighbors(Vector2 tile)
    {
        yield return tile + Vector2.up;
        yield return tile + Vector2.down;
        yield return tile + Vector2.left;
        yield return tile + Vector2.right;
    }

    protected IEnumerable<Vector2> GetTilesInRange(IEnumerable<Vector2> tiles, Vector2 center, int range)
    {
        foreach (var tile in tiles)
        {
            if (Vector2.Distance(tile, center) <= range)
            {
                yield return tile;
            }
        }
    }
    protected bool IsPositionAvailable(Vector2 position)
    {
        var grid = _PoolManager.Player.grid;
        if (grid.HasTile(new Vector2(position.x, position.y)))
        {
            if (grid._tiles[position]._entity.GetComponentInChildren<Entity>() == null)
            {
                return true;
            }
        }
        return false;
    }
}
