using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{
    public Stuff Stuff;
    public List<Item> Items;
    // Start is called before the first frame update
    public override void Start()
    {
        Debug.Log("Peniche");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Init()
    {
        Stuff = GetComponentInParent<Stuff>();
        SetBaseStats();

        List<Item> Items = new List<Item>();
        Items.Add(Stuff.Weapon);
        Items.Add(Stuff.Shield);
        Items.Add(Stuff.Head);
        Items.Add(Stuff.Torso);
        Items.Add(Stuff.Hands);
        Items.Add(Stuff.Legs);
        Items.Add(Stuff.Feets);
        Items.Add(Stuff.Trinket);
        foreach (var item in Items)
        {
            MaxHealth += item.Health;
            Damage += item.Damage;
            Defense += item.Defense;
            Speed += item.Speed;
            MoveDistance += item.MoveDistance;
            ActionPoints += item.ActionPoints;
        }
    }
    private void SetBaseStats()
    {
        MaxHealth = 3;
        Damage = 1;
        Defense = 1;
        Speed = 1;
        MoveDistance = 2;
        ActionPoints = 2;
    }
}
