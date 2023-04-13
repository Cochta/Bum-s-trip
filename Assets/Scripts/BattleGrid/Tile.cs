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

    public void Init(bool isOffset)
    {
        _col = GetComponent<BoxCollider2D>();
        _backgroundSR.color = isOffset ? _colorEven : _colorOdd;
        _originColor = _backgroundSR.color;
    }

    public void HighLight(Color color)
    {
        _backgroundSR.color = color;
    }

    private void OnMouseDown()
    {
        Ability selectedAbility = null;
        BattleManager bm = transform.parent.GetComponent<BattleManager>();
        foreach (var ability in bm.Pool.Abilities)
        {
            if (ability.IsSelected)
            {
                selectedAbility = ability;
            }

            ability.IsSelected = false;
            ability.IsOtherSelected = false;
        }
        if (selectedAbility != null && selectedAbility.Targets.Contains(Position - bm.Player.tile.Position))
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
        var display = transform.parent.GetComponent<EntityDisplay>();
        if (Entity.GetComponentInChildren<Entity>() != null)
        {
            display.Stats.SetActive(true);
            display.Display.OnDisplayEntity(Entity.GetComponentInChildren<Entity>());
        }

    }
    private void OnMouseExit()
    {
        _backgroundSR.color = _baseColor;
        transform.parent.GetComponent<EntityDisplay>().Stats.SetActive(false);
    }

    public void CanceHighlight()
    {
        _backgroundSR.color = _originColor;
        _baseColor = _originColor;
    }
}
