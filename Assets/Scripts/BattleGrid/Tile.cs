using UnityEngine;

public class Tile : MonoBehaviour
{
    public BoxCollider2D _col;

    public GameObject Entity;
    public GameObject Terrain;

    public Vector2 Position;
    [SerializeField] private SpriteRenderer _backgroundSR;
    [SerializeField] private Color _colorOdd;
    [SerializeField] private Color _colorEven;
    [SerializeField] private Color _colorHighlight;

    private Color _originColor;
    private Color _baseColor;

    private BattleManager _bm;
    private EntityDisplay _ed;

    public BattleManager BM { set => _bm = value; }
    public EntityDisplay ED { set => _ed = value; }

    public void Init(bool isOffset)
    {
        _col = GetComponent<BoxCollider2D>();
        _backgroundSR.color = isOffset ? _colorEven : _colorOdd;
        _originColor = _backgroundSR.color;
    }
    public void Init()
    {
        _originColor = _backgroundSR.color;
    }

    public void HighLight(Color color)
    {
        _backgroundSR.color = color;
    }

    private void OnMouseDown()
    {
        Ability selectedAbility = null;
        foreach (var ability in _bm.Pool.Abilities)
        {
            if (ability.IsSelected)
            {
                selectedAbility = ability;
            }

            ability.IsSelected = false;
            ability.IsOtherSelected = false;
        }
        if (selectedAbility != null && selectedAbility.Targets.Contains(Position - _bm.Player.tile.Position))
        {
            selectedAbility.PerformAction(this);
        }
        GetComponentInParent<Grid>().CancelHighlight();
        OnMouseExit();
    }

    private void OnMouseEnter()
    {
        _baseColor = _backgroundSR.color;
        _backgroundSR.color = _colorHighlight;
        if (Entity.GetComponentInChildren<Entity>() != null)
        {
            _ed.Stats.SetActive(true);
            _ed.Display.OnDisplayEntity(Entity.GetComponentInChildren<Entity>());
        }

    }
    private void OnMouseExit()
    {
        _backgroundSR.color = _baseColor;
        _ed.Stats.SetActive(false);
    }

    public void CanceHighlight()
    {
        _backgroundSR.color = _originColor;
        _baseColor = _originColor;
    }
}
