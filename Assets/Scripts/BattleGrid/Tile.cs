using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Entity _entity;

    public Vector2 _position;

    [SerializeField] private SpriteRenderer _entitySR;
    [SerializeField] private SpriteRenderer _backgroundSR;
    [SerializeField] private Color _colorOdd;
    [SerializeField] private Color _colorEven;
    [SerializeField] private Color _colorHighlight;

    private Color _originColor;


    public void Display()
    {
        if (_entity != null)
        {
            _entitySR.sprite = _entity._sprite;
        }
    }
    public void Init(bool isOffset)
    {
        _backgroundSR.color = isOffset ? _colorEven : _colorOdd;
        _originColor = _backgroundSR.color;
    }

    private void OnMouseEnter()
    {
        _backgroundSR.color = _colorHighlight;
    }
    private void OnMouseExit()
    {
        _backgroundSR.color = _originColor;
    }
}
