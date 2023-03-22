using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
    public Item _item;

    //[SerializeField] private TextMeshPro _description;

    [SerializeField] private SpriteRenderer _border;
    [SerializeField] private SpriteRenderer _itemSprite;

    [SerializeField] private Sprite _borderCommon;
    [SerializeField] private Sprite _borderUnCommon;
    [SerializeField] private Sprite _borderRare;
    [SerializeField] private Sprite _borderEpic;
    [SerializeField] private Sprite _borderLegendary;

    [SerializeField]
    private GameObject _stats;

    private void Update()
    {
        if (_item != null)
        {
            DisplayBorder();
            _itemSprite.sprite = _item._sprite;
        }
    }

    private void DisplayBorder()
    {
        if (_item._rarity == Item.Rarity.Common)
            _border.sprite = _borderCommon;
        else if (_item._rarity == Item.Rarity.UnCommon)
            _border.sprite = _borderUnCommon;
        else if (_item._rarity == Item.Rarity.Rare)
            _border.sprite = _borderRare;
        else if (_item._rarity == Item.Rarity.Epic)
            _border.sprite = _borderEpic;
        else if (_item._rarity == Item.Rarity.Legendary)
            _border.sprite = _borderLegendary;
    }

    private void OnMouseEnter()
    {
        if (_stats != null && _item != null)
        {
            _stats.SetActive(true);
            _stats.GetComponent<DisplayStats>().OnDisplayItem(_item);
            //_description.text = _item._description;
        }
    }

    private void OnMouseExit()
    {
        _stats.SetActive(false);
    }
}
