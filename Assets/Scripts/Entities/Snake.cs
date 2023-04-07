using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : Entity
{
    public override IEnumerator PerformAction()
    {
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
                if (PlayerInAttackRange() && !hasAttacked)
                {
                    hasAttacked = true;
                    Attack();
                    StartCoroutine(MoveToPositionThenReturn(transform, Player.transform.position, 0.5f));

                }
                else if (!PlayerInAttackRange() && !hasAttacked)
                    StartCoroutine(FollowPath(AStarSearch(Tile, Player.tile, Grid)));
                else if (PlayerInAttackRange() && hasAttacked)
                    StartCoroutine(FollowPath(Flee(Tile, Player.tile, Grid)));
                else
                    EndAction();

                yield return new WaitUntil(() => _currentActionPoints == wait - 1);
            }
        }
    }

}
