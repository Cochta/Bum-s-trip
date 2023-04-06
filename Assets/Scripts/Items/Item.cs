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
    [SerializeField] private bool _hasAbility = false;

    public Rarity _rarity = Rarity.Common;
    public Parts _part = Parts.None;

    [SerializeField] private Sprite sprite;

    public GameObject ability = null;


    public int Health { get => _health; set => _health = value; }
    public int Damage { get => _damage; set => _damage = value; }
    public int Defense { get => _defense; set => _defense = value; }
    public int Luck { get => _luck; set => _luck = value; }
    public int MoveDistance { get => _moveDistance; set => _moveDistance = value; }
    public int ActionPoints { get => _actionPoints; set => _actionPoints = value; }
    public string Name { get => _name; set => _name = value; }
    public string Description { get => _description; set => _description = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    public bool HasAbility { get => _hasAbility; set => _hasAbility = value; }

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
