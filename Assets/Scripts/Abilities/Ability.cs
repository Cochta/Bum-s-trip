using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public string Name;
    public string Description;

    [SerializeField] protected int _baseCooldown;
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
        if (RemainingCooldown > 0)
            Disable();
        PlayerData.Instance.UpdateData();
    }
    protected virtual void OnMouseEnter()
    {
        _poolManager.Display.Stats.SetActive(true);
        _poolManager.Display.Display.OnDisplayAbility(this);

        if (IsOtherSelected) return;

        foreach (var target in Targets)
        {
            var position = _poolManager.Player.tile.Position + target;
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
        _poolManager.Display.Stats.SetActive(false);
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
        bool canWalk = true;
        if (grid.HasTile(new Vector2(position.x, position.y)))
        {
            if (grid._tiles[position].Terrain.GetComponentInChildren<Terrain>() != null)
            {
                if (!grid._tiles[position].Terrain.GetComponentInChildren<Terrain>().IsWalkable)
                {
                    canWalk = false;
                }
            }
            if (grid._tiles[position].Entity.GetComponentInChildren<Entity>() == null && canWalk)
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
    protected IEnumerator MoveToPositionThenReturn(Transform transform, Vector3 position, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / (timeToMove / 2);
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
        t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / (timeToMove / 2);
            transform.position = Vector3.Lerp(position, currentPos, t);
            yield return null;
        }
        if (PlayerData.Instance.ActionsRemaining <= 0)
        {
            _poolManager.Player.IsPlayerTurn = false;
        }
    }
}
