using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMove : Ability
{
    protected override void Awake()
    {
        HighlightColor = Color.red;
        base.Awake();
    }
    protected override void OnMouseEnter()
    {
        Targets = new List<Vector2>();
        Targets.Add(new Vector2(1, 1));
        Targets.Add(new Vector2(1, 0));
        Targets.Add(new Vector2(1, -1));
        Targets.Add(new Vector2(-1, -1));
        Targets.Add(new Vector2(-1, 0));
        Targets.Add(new Vector2(-1, 1));
        Targets.Add(new Vector2(0, 1));
        Targets.Add(new Vector2(0, -1));

        base.OnMouseEnter();
    }

    public override void PerformAction(Tile tile)
    {
        if (tile._entity.GetComponentInChildren<Entity>() != null)
        {
            tile._entity.GetComponentInChildren<Entity>().TakeDamage(_PoolManager.Player.Damage);
        }
        base.PerformAction(tile);
    }
}
