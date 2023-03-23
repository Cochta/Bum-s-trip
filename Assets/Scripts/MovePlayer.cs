using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public class MovePlayer : Ability
{
    protected override void Awake()
    {
        HighlightColor = Color.blue;
        base.Awake();
    }
    protected override void OnMouseEnter()
    {
        Targets = new List<Vector2>();
        var player = _PoolManager.Player;

        var queue = new Queue<Vector2>();
        var distances = new Dictionary<Vector2, int> { { player.tile._position, 0 } };
        queue.Enqueue(player.tile._position);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            foreach (var direction in new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right })
            {
                var neighbor = current + direction;
                if (IsPositionAvailable(player.grid, neighbor) && !distances.ContainsKey(neighbor))
                {
                    distances.Add(neighbor, distances[current] + 1);
                    queue.Enqueue(neighbor);
                }
            }
        }

        foreach (var cell in distances)
        {
            if (cell.Value <= PlayerData.Instance.MoveDistance + 1)
            {
                Targets.Add(cell.Key - player.tile._position);
            }
        }

        base.OnMouseEnter();
    }

    private bool IsPositionAvailable(Grid grid, Vector2 position)
    {
        if (grid.HasTile(new Vector2(position.x, position.y)))
        {
            if (grid._tiles[position]._entity.GetComponentInChildren<Entity>() == null)
            {
                return true;
            }
        }
        return false;
    }
    public override void PerformAction(Tile tile)
    {
        PlayerInBattle p = _PoolManager.Player;
        p.transform.parent = tile._entity.transform;
        StartCoroutine(MoveToPosition(p.transform, tile._entity.transform.position, 0.5f));
        p.tile = tile;

        base.PerformAction(tile);
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
        if (PlayerData.Instance.ActionPoints <= 0)
        {
            _PoolManager.Player.IsPlayerTurn = false;
        }
    }
}
