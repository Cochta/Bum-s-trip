using System;
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
        Common = 1,
        UnCommon = 2,
        Rare = 3,
        Epic = 4,
        Legendary = 5
    }

    [SerializeField] private int _health = 0;
    [SerializeField] private int _damage = 0;
    [SerializeField] private int _defense = 0;
    [SerializeField] private int _luck = 0;

    [NonSerialized] public int Health = 0;
    [NonSerialized] public int Damage = 0;
    [NonSerialized] public int Defense = 0;
    [NonSerialized] public int Luck = 0;
    public int MoveDistance = 0; // nb of tiles it can move
    public int ActionPoints = 0;


    public string Name = null;
    public string Description = null;

    public Rarity _rarity = Rarity.Common;
    public Parts _part = Parts.None;

    public Sprite _sprite;

    public Ability ability = null;

    public void SetStats()
    {
        Health = _health * (int)_rarity;
        Damage = _damage * (int)_rarity;
        Defense = _defense * (int)_rarity;
        Luck = _luck * (int)_rarity;
    }

    protected virtual void OnAttack()
    {

    }
    protected virtual void OnTakeHit()
    {

    }
    protected virtual void Ability() //prend la case en argument
    {

    }
    protected virtual void Passiv()
    {

    }
}
