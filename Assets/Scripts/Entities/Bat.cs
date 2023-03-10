using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bat : Entity
{
    private Dictionary<Vector2, Tile> grid;

    private void Update()
    {
        if (PlayerNearby() != null)
        {
            Debug.Log("Attack");
        }
    }
    public override void Start()
    {
        Debug.Log("Peniche");
    }
    protected override void PerformAction()
    {
        for (int i = 0; i < ActionPoints; i++)
        {
            grid = GetGrid();
            if (PlayerNearby() != null)
            {
                Attack(PlayerNearby()._entity, Damage);
            }
            else
            {
                Move();
            }

        }
    }
    protected override void Move()
    {
        base.Move();
    }
    private Tile PlayerNearby()
    {
        List<Vector2> targets = new List<Vector2>();
        targets.Add(new Vector2(1, 1));
        targets.Add(new Vector2(1, 0));
        targets.Add(new Vector2(1, -1));
        targets.Add(new Vector2(-1, -1));
        targets.Add(new Vector2(-1, 0));
        targets.Add(new Vector2(-1, 1));
        targets.Add(new Vector2(0, 1));
        targets.Add(new Vector2(0, -1));

        foreach (var target in targets)
        {
            if (grid[target]._entity.tag == "Player")
            {
                Debug.Log(grid[target]._entity.tag);
                return grid[target];
            }
        }
        return null;
        //if (grid[new Vector2(tile._position.x + 1, tile._position.y + 1)]._entity != null &&
        //    grid[new Vector2(tile._position.x + 1, tile._position.y + 1)]._entity.tag == "Player")
        //{
        //    return grid[new Vector2(tile._position.x + 1, tile._position.y + 1)];
        //}
        //else if (grid[new Vector2(tile._position.x + 1, tile._position.y)]._entity != null &&
        //    grid[new Vector2(tile._position.x + 1, tile._position.y)]._entity.tag == "Player")
        //{
        //    return grid[new Vector2(tile._position.x + 1, tile._position.y)];
        //}
        //else if (grid[new Vector2(tile._position.x + 1, tile._position.y - 1)]._entity != null &&
        //    grid[new Vector2(tile._position.x + 1, tile._position.y - 1)]._entity.tag == "Player")
        //{
        //    return grid[new Vector2(tile._position.x + 1, tile._position.y - 1)];
        //}
        //else if (grid[new Vector2(tile._position.x - 1, tile._position.y - 1)]._entity != null &&
        //    grid[new Vector2(tile._position.x - 1, tile._position.y - 1)]._entity.tag == "Player")
        //{
        //    return grid[new Vector2(tile._position.x - 1, tile._position.y - 1)];
        //}
        //else if (grid[new Vector2(tile._position.x - 1, tile._position.y)]._entity != null &&
        //    grid[new Vector2(tile._position.x - 1, tile._position.y)]._entity.tag == "Player")
        //{
        //    return grid[new Vector2(tile._position.x - 1, tile._position.y)];
        //}
        //else if (grid[new Vector2(tile._position.x - 1, tile._position.y + 1)]._entity != null &&
        //    grid[new Vector2(tile._position.x - 1, tile._position.y + 1)]._entity.tag == "Player")
        //{
        //    return grid[new Vector2(tile._position.x, tile._position.y + 1)];
        //}
        //else if (grid[new Vector2(tile._position.x, tile._position.y - 1)]._entity != null &&
        //    grid[new Vector2(tile._position.x, tile._position.y - 1)]._entity.tag == "Player")
        //{
        //    return grid[new Vector2(tile._position.x, tile._position.y - 1)];
        //}
        //else if (grid[new Vector2(tile._position.x, tile._position.y + 1)]._entity != null &&
        //    grid[new Vector2(tile._position.x, tile._position.y + 1)]._entity.tag == "Player")
        //{
        //    return grid[new Vector2(tile._position.x, tile._position.y + 1)];
        //}
        //else { return null; }
    }
}
