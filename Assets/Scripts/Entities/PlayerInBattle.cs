using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static BattleManager;
using static UnityEngine.GraphicsBuffer;

public class PlayerInBattle : MonoBehaviour
{
    public Tile tile;
    public Grid grid;


    public bool Isdead = false;
    public Stuff Stuff;
    public List<Item> Items = new List<Item>();

    public bool IsPlayerTurn = false;

    protected void Awake()
    {
        GetGrid();
        tile = GetTile();
    }

    void Update()
    {
        if (IsPlayerTurn) PlayerData.Instance.EnableAbilities();
        else PlayerData.Instance.DisableAbilities();

    }
    private void GetGrid()
    {
        grid = transform.parent.parent.GetComponentInParent<Grid>();
    }

    private Tile GetTile()
    {
        return transform.parent.GetComponentInParent<Tile>();
    }
}
