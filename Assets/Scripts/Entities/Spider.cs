using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Entity
{
    private List<Vector2> _safeZoneTargets;

    [SerializeField] private GameObject _webPrefab;

    private bool IsPlayerInSafeZone()
    {
        foreach (var target in _safeZoneTargets)
        {
            if (Grid.ContainsKey(Tile.Position + target))
            {
                if (Grid[Tile.Position + target].GetComponentInChildren<PlayerInBattle>() != null)
                {
                    return true;
                }
            }
        }

        return false;
    }
    public override IEnumerator PerformAction()
    {
        _safeZoneTargets = SetTargets(2);
        _attackTargets = SetTargets(_attackRange);
        _currentActionPoints = BaseActionPoints;

        if (IsDead)
        {
            IsTurn = false;
        }
        else
        {
            bool hasAttacked = false;
            for (int i = 0; i < BaseActionPoints; i++)
            {
                int wait = _currentActionPoints;
                if (IsPlayerInSafeZone())
                {
                    StartCoroutine(FollowPath(Flee(Tile, Player.tile, Grid)));
                }
                else if (PlayerInAttackRange() && !hasAttacked)
                {
                    hasAttacked = true;
                    Attack();
                    StartCoroutine(ShootAtPlayer(0.5f)); //  shoot web
                }
                else if (!PlayerInAttackRange() && !hasAttacked)
                    StartCoroutine(FollowPath(AStarSearch(Tile, Player.tile, Grid)));
                else
                    EndAction();

                yield return new WaitUntil(() => _currentActionPoints == wait - 1);
            }
        }
    }
    public IEnumerator ShootAtPlayer(float timeToShoot)
    {
        var web = Instantiate(_webPrefab, Player.tile.Terrain.transform);

        foreach (var target in _attackTargets)
        {
            if (Grid.ContainsKey(Tile.Position + target))
                Grid[Tile.Position + target].HighLight(Color.red);
        }

        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToShoot;
            web.transform.position = Vector3.Lerp(currentPos, Player.tile.transform.position, t);
            yield return null;
        }
        SpawnedEntities.Add(web);
        EndAction();
    }
}
