using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static BattleManager;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    public int CurentHealth = 0;
    public int MaxHealth = 0;
    public int Damage = 0;
    public int Defense = 0;
    public int Luck = 0;
    public int MoveDistance = 0; // nb of tiles it can move
    public int ActionPoints = 0;

    public Tile tile;
    public Dictionary<Vector2, Tile> grid;

    public string Name = null;


    public bool Isdead = false;
    public Stuff Stuff;
    public List<Item> Items = new List<Item>();

    public int LeftActions;

    public PlayerDisplay _display;

    public BattleManager BM;

    protected void Awake()
    {
        GetGrid();
        tile = GetTile();
        LeftActions = ActionPoints;
    }

    private void Update()
    {
        if (ActionPoints <= 0)
        {
            BM.ChangeState(GameStates.EnemiesTurn);
            ActionPoints = 2;
        }
    }

    public void Attack(Entity entity, int Damage)
    {
        entity.TakeDamage(Damage);
        _display.OnDisplayPlayer(this);
    }
    public void TakeDamage(int Damage)
    {
        CurentHealth -= Damage - Defense;
        _display.OnDisplayPlayer(this);
    }
    private void GetGrid()
    {
        grid = transform.parent.parent.GetComponentInParent<Grid>()._tiles;
    }

    private Tile GetTile()
    {
        return transform.parent.GetComponentInParent<Tile>();
    }

    public void Init(Stuff stuff, PlayerDisplay display)
    {
        Stuff = stuff;
        _display = display;
        SetBaseStats();

        if (Stuff.Weapon != null)
            Items.Add(Stuff.Weapon);
        if (Stuff.Shield != null)
            Items.Add(Stuff.Shield);
        if (Stuff.Head != null)
            Items.Add(Stuff.Head);
        if (Stuff.Torso != null)
            Items.Add(Stuff.Torso);
        if (Stuff.Hands != null)
            Items.Add(Stuff.Hands);
        if (Stuff.Legs != null)
            Items.Add(Stuff.Legs);
        if (Stuff.Feets != null)
            Items.Add(Stuff.Feets);
        if (Stuff.Trinket != null)
            Items.Add(Stuff.Trinket);
        foreach (var item in Items)
        {
            MaxHealth += item.Health;
            Damage += item.Damage;
            Defense += item.Defense;
            Luck += item.Luck;
            MoveDistance += item.MoveDistance;
            ActionPoints += item.ActionPoints;
        }
        _display.OnDisplayPlayer(this);
    }
    private void SetBaseStats()
    {
        MaxHealth = 3;
        Damage = 1;
        Defense = 1;
        Luck = 1;
        MoveDistance = 2;
        ActionPoints = 2;
    }
}
