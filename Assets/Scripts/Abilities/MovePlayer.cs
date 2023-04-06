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
        _highlightColor = Color.blue;
        base.Awake();
    }
    protected override void OnMouseEnter()
    {
        Targets = new List<Vector2>();
        var playerPos = _poolManager.Player.tile._position;

        var visited = new HashSet<Vector2>();
        var queue = new Queue<Vector2>();
        var distances = new Dictionary<Vector2, int> { { playerPos, 0 } };

        queue.Enqueue(playerPos);
        visited.Add(playerPos);

        while (queue.Count > 0)
        {
            var currentTile = queue.Dequeue();
            var currentDistance = distances[currentTile];

            foreach (var neighborTile in GetNeighbors(currentTile))
            {
                if (visited.Contains(neighborTile)) continue;
                var neighborDistance = currentDistance + 1;

                if (neighborDistance > PlayerData.Instance.MoveDistance) break;

                if (!IsPositionAvailable(neighborTile)) continue;

                visited.Add(neighborTile);
                distances[neighborTile] = neighborDistance;
                queue.Enqueue(neighborTile);
            }
        }

        foreach (var targetTile in GetTilesInRange(distances.Keys, playerPos, PlayerData.Instance.MoveDistance))
        {
            Targets.Add(targetTile - playerPos);
        }

        base.OnMouseEnter();
    }

    public override void PerformAction(Tile tile)
    {
        PlayerInBattle p = _poolManager.Player;
        p.transform.parent = tile._entity.transform;
        StartCoroutine(MoveToPosition(p.transform, tile._entity.transform.position, 0.5f));
        p.tile = tile;

        PlayerData.Instance.ActionPoints -= 1;

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
            _poolManager.Player.IsPlayerTurn = false;
        }
    }
}
