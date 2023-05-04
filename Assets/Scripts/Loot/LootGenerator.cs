using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _itemHolderPrefab;
    [SerializeField] private GameObject _displayStats;
    [SerializeField] private ItemPool _itemPool;
    [SerializeField] private Stuff _stuff;
    private List<GameObject> _itemHoldersList = new List<GameObject>();
    [SerializeField] private GameObject _shopItemCostPrefab;

    [SerializeField] private SpriteRenderer _backgroundSR;
    [SerializeField] private GameObject _button;

    private System.Random _rnd = new System.Random();

    public Item SpecificItem = null;

    public void GenerateLoot()
    {
        DisplayState(true);
        // Remove existing item holders
        foreach (var itemHolder in _itemHoldersList)
        {
            Destroy(itemHolder);
        }
        _itemHoldersList.Clear();

        // Calculate the number of items and item level based on the current state and node type

        int nbItems = 1;
        bool isShop = PlayerData.Instance.Node.Type == PlayerData.GameStates.ToShop;
        bool isTreasure = PlayerData.Instance.Node.Type == PlayerData.GameStates.ToTreasure;
        if (isShop || isTreasure)
            nbItems = 3;
        int itemLevel = PlayerData.Instance.Level;
        bool isBossLoot = PlayerData.Instance.State == PlayerData.GameStates.ToBossLoot;
        if (isBossLoot)
        {
            itemLevel++;
        }
        if (SpecificItem != null)
        {
            nbItems = 1;
            DisplayState(false);
        }

        // Generate item holders
        float startX = -nbItems + 1f;
        for (int i = 0; i < nbItems; i++)
        {
            // Instantiate item holder prefab
            var obj = Instantiate(_itemHolderPrefab, transform);
            _itemHoldersList.Add(obj);
            obj.transform.localPosition = new Vector3(startX + i * 2f, 0.3f, 0f);

            // Set item display properties
            var display = obj.GetComponent<ItemDisplay>();
            display.Stuff = _stuff;
            display.Stats = _displayStats;
            display.IsLoot = true;

            // Select random item from the pool
            if (SpecificItem != null)
            {
                display._item = SpecificItem;
            }
            else
            {
                var itemList = new List<Item>(_itemPool.Items);
                var randomItem = _rnd.Next(0, itemList.Count);
                display._item = Instantiate(itemList[randomItem]);
                itemList.RemoveAt(randomItem);
            }


            // Set item rarity based on the item level and luck
            var randomLevel = _rnd.Next(0, 101);
            if (randomLevel >= 0 && randomLevel <= 5 && itemLevel > (int)Item.Rarity.Common)
            {
                display._item._rarity = (Item.Rarity)(itemLevel - 1);
            }
            else if (randomLevel >= 100 - PlayerData.Instance.Luck && randomLevel <= 100 &&
                     itemLevel < (int)Item.Rarity.Legendary)
            {
                display._item._rarity = (Item.Rarity)(itemLevel + 1);
            }
            else
            {
                display._item._rarity = (Item.Rarity)itemLevel;
            }

            // Set item stats and add to the display list
            display._item.SetStats();
        }

        // If it's a shop, set the rarity of each item holder
        if (isShop)
        {
            SetShopItem(_itemHoldersList[0], itemLevel > (int)Item.Rarity.Common ? (Item.Rarity)itemLevel - 1 : (Item.Rarity)itemLevel);
            SetShopItem(_itemHoldersList[1], (Item.Rarity)itemLevel);
            SetShopItem(_itemHoldersList[2], itemLevel < (int)Item.Rarity.Legendary ? (Item.Rarity)itemLevel + 1 : (Item.Rarity)itemLevel);
        }
        SpecificItem = null;
    }

    void SetShopItem(GameObject itemHolder, Item.Rarity rarity)
    {
        var itemDiplay = itemHolder.GetComponent<ItemDisplay>();
        itemDiplay._item._rarity = rarity;
        itemDiplay._item.SetStats();
        itemDiplay.Cost = SetCost(itemDiplay._item);
        var shopCost = Instantiate(_shopItemCostPrefab, itemDiplay.transform).GetComponent<ShopItemCost>();
        shopCost.Value = itemDiplay.Cost;
    }

    private int SetCost(Item item)
    {
        int itemRarityLevel = (int)item._rarity;
        int playerLevel = PlayerData.Instance.Level;

        if (itemRarityLevel < playerLevel) return 7;
        else if (itemRarityLevel > playerLevel) return 30;
        else return 15;
    }
    public void DisplayState(bool activestate)
    {
        _backgroundSR.enabled = activestate;
        _button.SetActive(activestate);
    }
}
