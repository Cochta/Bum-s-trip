using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bat : Entity
{
    List<Vector2> targets;
    protected override void Awake()
    {
        GetGrid();
        tile = GetTile();
        SetTargets();
    }
    private void SetTargets()
    {
        targets = new List<Vector2>();
        targets.Add(new Vector2(1, 1));
        targets.Add(new Vector2(1, 0));
        targets.Add(new Vector2(1, -1));
        targets.Add(new Vector2(-1, -1));
        targets.Add(new Vector2(-1, 0));
        targets.Add(new Vector2(-1, 1));
        targets.Add(new Vector2(0, 1));
        targets.Add(new Vector2(0, -1));
    }

    public override void PerformAction()
    {
        if (!Isdead)
            StartCoroutine(ActionList());
    }

    private IEnumerator ActionList()
    {

        for (int i = 0; i < ActionPoints; i++)
        {
            yield return new WaitForSeconds(1);
            if (PlayerNearby() != null)
            {
                Attack(PlayerNearby(), Damage);
                //Debug.Log("bat taper");
            }
            else
            {
                Move();
            }
        }

    }

    protected override void Move()
    {
        var pPos = Player.GetComponent<Player>().tile._position;
        var myPos = tile._position;
        float y = 0;
        float x = 0;
        bool diagonale = Mathf.Abs(myPos.y - pPos.y) == Mathf.Abs(myPos.x - pPos.x);

        if (myPos.y > pPos.y)
        {
            y = myPos.y - 1;
            x = myPos.x;
            if (diagonale && (myPos.x > pPos.x))
            {
                x = myPos.x - 1;
            }
            else if (diagonale && (myPos.x < pPos.x))
            {
                x = myPos.x + 1;
            }
        }
        else if (myPos.y < pPos.y)
        {
            y = myPos.y + 1;
            x = myPos.x;
            if (diagonale && (myPos.x > pPos.x))
            {
                x = myPos.x - 1;
            }
            else if (diagonale && (myPos.x < pPos.x))
            {
                x = myPos.x + 1;
            }
        }
        else if (myPos.y == pPos.y)
        {
            if (myPos.x > pPos.x)
            {
                y = myPos.y;
                x = myPos.x - 1;
            }
            else if (myPos.x < pPos.x)
            {
                y = myPos.y;
                x = myPos.x + 1;
            }
        }

        var newPos = new Vector2(x, y);

        if (grid[newPos]._entity.GetComponentInChildren<Entity>() == null)
        {
            transform.parent = grid[newPos]._entity.transform;
            transform.position = transform.parent.position;
            tile = grid[newPos];
        }
        tile = GetTile();
    }

    private Player PlayerNearby()
    {
        foreach (var target in targets)
        {
            Tile value;
            if (grid.TryGetValue(tile._position + target, out value))
            {
                if (grid[tile._position + target].GetComponentInChildren<Player>() != null)
                {
                    return grid[tile._position + target].GetComponentInChildren<Player>();
                }
            }
        }
        return null;
    }
}
