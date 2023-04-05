using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stuff : MonoBehaviour
{

    public Item Weapon;
    public Item Shield;
    public Item Head;
    public Item Torso;
    public Item Hands;
    public Item Legs;
    public Item Feets;
    public Item Trinket;

    public void AddItem(Item newItem)
    {
        if (newItem != null)
        {
            if (newItem._part == Item.Parts.Weapon)
                Weapon = newItem;
            else if (newItem._part == Item.Parts.Shield)
                Shield = newItem;
            else if (newItem._part == Item.Parts.Head)
                Head = newItem;
            else if (newItem._part == Item.Parts.Hands)
                Hands = newItem;
            else if (newItem._part == Item.Parts.Torso)
                Torso = newItem;
            else if (newItem._part == Item.Parts.Legs)
                Legs = newItem;
            else if (newItem._part == Item.Parts.Feets)
                Feets = newItem;
            else if (newItem._part == Item.Parts.Trinket)
                Trinket = newItem;

            PlayerData.Instance.UpdateData();
        }
    }
}
