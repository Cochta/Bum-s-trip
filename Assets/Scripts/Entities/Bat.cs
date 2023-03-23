using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
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
        {
            for (int i = 0; i < ActionPoints; i++)
            {
                if (PlayerNearby() != null)
                {
                    Attack(PlayerNearby(), Damage);
                    StartCoroutine(MoveToPositionThenReturn(transform, Player.transform.position, 0.5f));
                    //Debug.Log("bat taper");
                }
                else
                {
                    Move();
                }
            }
        }
    }
    protected override void Move()
    {
        var playerTile = Player.GetComponent<PlayerInBattle>().tile._position;
        var myTile = tile._position;
        Vector2 newPos = Vector2.zero;

        if (myTile.y > playerTile.y)
        {
            newPos = new Vector2(myTile.x, myTile.y - 1);
            if (Mathf.Abs(myTile.y - playerTile.y) == Mathf.Abs(myTile.x - playerTile.x))
            {
                newPos.x = myTile.x > playerTile.x ? myTile.x - 1 : myTile.x + 1;
            }
        }
        else if (myTile.y < playerTile.y)
        {
            newPos = new Vector2(myTile.x, myTile.y + 1);
            if (Mathf.Abs(myTile.y - playerTile.y) == Mathf.Abs(myTile.x - playerTile.x))
            {
                newPos.x = myTile.x > playerTile.x ? myTile.x - 1 : myTile.x + 1;
            }
        }
        else if (myTile.y == playerTile.y)
        {
            newPos = new Vector2(myTile.x < playerTile.x ? myTile.x + 1 : myTile.x - 1, myTile.y);
        }

        if (grid[newPos]._entity.GetComponentInChildren<Entity>() == null)
        {
            transform.parent = grid[newPos]._entity.transform;
            tile = grid[newPos];
        }
        StartCoroutine(MoveToPosition(transform, transform.parent.position, 0.5f));
        tile = GetTile();
        HasFinishedTurn = true;
    }
    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
        IsTurn = false;
    }
    public IEnumerator MoveToPositionThenReturn(Transform transform, Vector3 position, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / (timeToMove / 2);
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
        t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / (timeToMove / 2);
            transform.position = Vector3.Lerp(position, currentPos, t);
            yield return null;
        }
        IsTurn = false;
    }

    private PlayerInBattle PlayerNearby()
    {
        foreach (var target in targets)
        {
            Tile value;
            if (grid.TryGetValue(tile._position + target, out value))
            {
                if (grid[tile._position + target].GetComponentInChildren<PlayerInBattle>() != null)
                {
                    return grid[tile._position + target].GetComponentInChildren<PlayerInBattle>();
                }
            }
        }
        return null;
    }
}
