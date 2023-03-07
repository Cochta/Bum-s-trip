using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public enum Parts
    {
        Weapon,
        Shield,
        Head,
        Torso,
        Hands,
        Legs,
        Feets,
        Trinket,
        None
    }
    public enum Rarity
    {
        Common,
        UnCommon,
        Rare,
        Epic,
        Legendary
    }

    public int _health = 0;
    public int _attack = 0;
    public int _defense = 0;
    public int _dodge = 0;
    public int _crit = 0;
    public int _speed = 0;

    public string _description = null;

    public Rarity _rarity = Rarity.Common;
    public Parts _part = Parts.None;

    public Sprite _sprite;



    protected virtual void OnAttack()
    {
        
    }

    protected abstract void OnTakeHit();
}
