using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ability : MonoBehaviour
{
    [SerializeField] private int _baseCooldown;
    private int _remainingCooldown = 0;

    protected MovePoolManager _poolManager;
    protected Color _highlightColor = Color.green;

    private BoxCollider2D _col;
    public bool IsSelected { get; set; }
    public bool IsOtherSelected { get; set; }
    public int RemainingCooldown { get => _remainingCooldown; set => _remainingCooldown = value; }
    public List<Vector2> Targets { get; set; }

    private SpriteRenderer _sr;

    private TextMeshPro _text;

    protected virtual void Awake()
    {
        _poolManager = GetComponentInParent<MovePoolManager>();
        _col = GetComponent<BoxCollider2D>();
        _sr = GetComponent<SpriteRenderer>();
        _text = GetComponentInChildren<TextMeshPro>();
        _text.enabled = false;
    }
    public virtual void PerformAction(Tile tile)
    {
        IsSelected = false;
        tile.GetComponentInParent<Grid>().CancelHighlight();
        RemainingCooldown = _baseCooldown;
        if (RemainingCooldown > 0)
            Disable();
    }
    protected virtual void OnMouseEnter()
    {
        if (IsOtherSelected) return;

        foreach (var target in Targets)
        {
            var position = _poolManager.Player.tile._position + target;
            if (_poolManager.Player.grid._tiles.TryGetValue(position, out var tile))
            {
                tile.HighLight(_highlightColor);
            }
        }
    }
    protected virtual void OnMouseDown()
    {
        if (IsOtherSelected)
        {
            CancelTileHighlights();
            IsOtherSelected = false;
            OnMouseEnter();
        }
        foreach (var ability in _poolManager.Abilities)
        {
            ability.IsSelected = false;
            ability.IsOtherSelected = true;
        }
        IsSelected = true;
    }
    protected virtual void OnMouseExit()
    {
        if (!IsSelected && !IsOtherSelected)
        {
            CancelTileHighlights();
        }
    }
    private void CancelTileHighlights()
    {
        foreach (var tile in _poolManager.Player.grid._tiles.Values)
        {
            tile.CanceHighlight();
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
        var grid = _poolManager.Player.grid;
        if (grid.HasTile(new Vector2(position.x, position.y)))
        {
            if (grid._tiles[position]._entity.GetComponentInChildren<Entity>() == null)
            {
                return true;
            }
        }
        return false;
    }
    public void SetSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
    public void Enable()
    {
        if (RemainingCooldown <= 0)
        {
            _text.enabled = false;
            _col.enabled = true;
            _sr.color = Color.white;
        }
        else
        {
            _text.text = RemainingCooldown.ToString();
        }
    }
    public void Disable()
    {
        if (RemainingCooldown > 0)
        {
            _text.enabled = true;
            _text.text = RemainingCooldown.ToString();
        }
        _col.enabled = false;
        _sr.color = Color.grey;
    }
}
