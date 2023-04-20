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
    [SerializeField] private int _moveDistance = 0; // nb of tiles PLAYER can move
    [SerializeField] private int _actionPoints = 0;
    [SerializeField] private string _name = null;
    [SerializeField] private string _description = null;

    public Rarity _rarity = Rarity.Common;
    public Parts _part = Parts.None;

    [SerializeField] private Sprite sprite;

    public GameObject ability = null;


    [NonSerialized] public int Health;
    [NonSerialized] public int Damage;
    [NonSerialized] public int Defense;
    [NonSerialized] public int Luck;
    [NonSerialized] public int MoveDistance;
    [NonSerialized] public int ActionPoints;
    public string Name { get => _name; set => _name = value; }
    public string Description { get => _description; set => _description = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }

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
    protected virtual void Passiv()
    {

    }
}
