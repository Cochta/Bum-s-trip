using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMove : Ability
{
    protected override void Awake()
    {
        _highlightColor = Color.red;
        base.Awake();
    }
    protected override void OnMouseEnter()
    {
        Description = "Deal " + PlayerData.Instance.Damage + " damages !";
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
        if (tile.Entity.GetComponentInChildren<Entity>() != null)
        {
            StartCoroutine(MoveToPositionThenReturn(_poolManager.Player.transform, tile.transform.position, 0.5f));
            tile.Entity.GetComponentInChildren<Entity>().TakeDamage(PlayerData.Instance.Damage);
            PlayerData.Instance.ActionsRemaining -= 1;
        }
        base.PerformAction(tile);
    }
}
