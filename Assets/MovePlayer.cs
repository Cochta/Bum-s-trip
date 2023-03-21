using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
        List<Vector2> baseTargets = new List<Vector2>();
        baseTargets.Add(new Vector2(1, 0));
        baseTargets.Add(new Vector2(-1, 0));
        baseTargets.Add(new Vector2(0, 1));
        baseTargets.Add(new Vector2(0, -1));

        Tile value;
        for (int i = 0; i < _PoolManager.Player.MoveDistance; i++)
        {
            foreach (var target1 in baseTargets)
            {
                foreach (var target2 in baseTargets)
                {
                    if (!Targets.Contains(target1 + (target2 * i)) && (target1 + (target2 * i)) != Vector2.zero)
                    {
                        if (_PoolManager.Player.grid.TryGetValue(_PoolManager.Player.tile._position + target1 + (target2 * i), out value))
                        {
                            if (_PoolManager.Player.grid[_PoolManager.Player.tile._position + target1 + (target2 * i)]._entity.GetComponentInChildren<Entity>() == null)
                            {
                                Targets.Add(target1 + (target2 * i));
                            }
                        }
                    }
                }
            }
        }
        base.OnMouseEnter();
    }

    public override void PerformAction(Tile tile)
    {
        Player p = _PoolManager.Player;
        p.transform.parent = tile._entity.transform;
        p.transform.position = tile._entity.transform.position;
        p.tile = tile;

        base.PerformAction(tile);
    }
}
