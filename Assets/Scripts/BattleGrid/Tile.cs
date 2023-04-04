using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public BoxCollider2D _col;

    public GameObject _entity;

    public Vector2 _position;

    [SerializeField] private SpriteRenderer _entitySR;
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

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
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
            }
            if (selectedAbility != null && selectedAbility.Targets.Contains(_position - bm.Player.tile._position))
            {
                selectedAbility.PerformAction(this);
            }
            GetComponentInParent<Grid>().CancelHighlight();
            OnMouseExit();
        }
    }

    private void OnMouseEnter()
    {
        _baseColor = _backgroundSR.color;
        _backgroundSR.color = _colorHighlight;
        var display = transform.parent.GetComponent<EntityDisplay>();
        if (_entity.GetComponentInChildren<Entity>() != null)
        {
            display.Stats.SetActive(true);
            display.Display.OnDisplayEntity(_entity.GetComponentInChildren<Entity>());
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
