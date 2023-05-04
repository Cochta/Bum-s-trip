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

    public GameObject Stats;
    [SerializeField] private SpriteRenderer _backGroundSR;

    public bool IsLoot = false;
    public int Cost = 0;

    public Stuff Stuff;

    private void Update()
    {
        if (_item != null)
        {
            DisplayBorder();
            _itemSprite.sprite = _item.Sprite;
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
        _backGroundSR.enabled = true;
        if (Stats != null && _item != null)
        {
            Stats.SetActive(true);
            Stats.GetComponent<DisplayStats>().OnDisplayItem(_item);
        }
    }

    private void OnMouseDown()
    {
        if (PlayerData.Instance.Coins >= Cost && IsLoot)
        {
            Stuff.AddItem(_item);
            SoundHandeler.Instance.PlayBumGetItem();
            if (PlayerData.Instance.State == PlayerData.GameStates.ToBossLoot)
                PlayerData.Instance.ChangeGameState(PlayerData.GameStates.EnterNewLevel);
            else
                PlayerData.Instance.ChangeGameState(PlayerData.GameStates.ToMap);
            if (Cost != 0)
                PlayerData.Instance.LoseMoney(Cost);
            OnMouseExit();
        }
    }

    private void OnMouseExit()
    {
        Stats.SetActive(false);
        _backGroundSR.enabled = false;
    }
}
