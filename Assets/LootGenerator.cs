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

    private System.Random _rnd = new System.Random();

    public void GenerateLoot()
    {
        if (_itemHoldersList.Any())
        {
            foreach (var itemHolder in _itemHoldersList)
            {
                Destroy(itemHolder);
            }
        }

        bool isShop = false;
        int nbItems = 1;
        int itemLevel = PlayerData.Instance.Level;

        if (PlayerData.Instance.Node.Type == Node.NodeTypes.Enemy)
            nbItems = 1;
        else if (PlayerData.Instance.Node.Type == Node.NodeTypes.Treasure)
            nbItems = 3;
        else if (PlayerData.Instance.Node.Type == Node.NodeTypes.Shop)
        {
            nbItems = 3;
            isShop = true;
        }
        else if (PlayerData.Instance.Node.Type == Node.NodeTypes.Boss)
        {
            nbItems = 1;
            itemLevel++;
        }


        float totalWidth = nbItems * 2f;
        float startX = -totalWidth / 2f + 1f;

        List<Item> itemList = new List<Item>(_itemPool.Items);
        _itemHoldersList = new List<GameObject>();

        for (int i = 0; i < nbItems; i++)
        {

            var randomItem = _rnd.Next(0, itemList.Count);
            var randomLevel = _rnd.Next(0, 101);

            var obj = Instantiate(_itemHolderPrefab, transform);
            _itemHoldersList.Add(obj);
            var display = obj.GetComponent<ItemDisplay>();
            display.Stuff = _stuff;
            display._item = itemList[randomItem];
            itemList.RemoveAt(randomItem);
            display.Stats = _displayStats;
            display.IsLoot = true;


            if (randomLevel >= 0 && randomLevel <= 5 && itemLevel > (int)Item.Rarity.Common)
                display._item._rarity = (Item.Rarity)(itemLevel - 1);
            else if (randomLevel >= 100 - PlayerData.Instance.Luck && randomLevel <= 100 &&
                     itemLevel < (int)Item.Rarity.Legendary)
                display._item._rarity = (Item.Rarity)(itemLevel + 1);
            else
                display._item._rarity = (Item.Rarity)itemLevel;

            display._item.SetStats();

            obj.transform.localPosition = new Vector3(startX + i * 2f, 0.3f, 0f);
        }

        if (isShop)
        {
            SetShopItem(_itemHoldersList[0], itemLevel > (int)Item.Rarity.Common ? (Item.Rarity)itemLevel - 1 : (Item.Rarity)itemLevel);
            SetShopItem(_itemHoldersList[1], (Item.Rarity)itemLevel);
            SetShopItem(_itemHoldersList[2], itemLevel < (int)Item.Rarity.Legendary ? (Item.Rarity)itemLevel + 1 : (Item.Rarity)itemLevel);
        }

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
        if ((int)item._rarity < PlayerData.Instance.Level)
            return 7;
        else if ((int)item._rarity > PlayerData.Instance.Level)
            return 30;

        return 15;
    }

}
