using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
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
        var movedistance = PlayerData.Instance.MoveDistance;
        if (_poolManager.Player.tile.Terrain.GetComponentInChildren<Terrain>() != null)
        {
            if (_poolManager.Player.tile.Terrain.GetComponentInChildren<Web>() != null)
            {
                movedistance -= 1;
            }
        }

        Description = "Moves " + movedistance + " tiles !";
        Targets = new List<Vector2>();
        var playerPos = _poolManager.Player.tile.Position;

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

                if (neighborDistance > movedistance) break;

                if (!IsPositionAvailable(neighborTile)) continue;

                visited.Add(neighborTile);
                distances[neighborTile] = neighborDistance;
                queue.Enqueue(neighborTile);
            }
        }

        foreach (var targetTile in GetTilesInRange(distances.Keys, playerPos, movedistance))
        {
            Targets.Add(targetTile - playerPos);
        }
        Targets.Remove(Vector2.zero);
        base.OnMouseEnter();
    }

    public override void PerformAction(Tile tile)
    {
        PlayerData.Instance.ActionsRemaining -= 1;
        PlayerInBattle p = _poolManager.Player;
        p.transform.parent = tile.Entity.transform;
        StartCoroutine(MoveToPosition(p.transform, tile.Entity.transform.position, 0.5f));
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
        if (PlayerData.Instance.ActionsRemaining <= 0)
        {
            _poolManager.Player.IsPlayerTurn = false;
        }
    }
}
