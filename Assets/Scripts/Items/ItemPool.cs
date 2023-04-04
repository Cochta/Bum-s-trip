using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemPool", menuName = "ScriptableObjects/ItemPool", order = 1)]

public class ItemPool : ScriptableObject
{
    public List<Item> Items = new List<Item>();
}
