using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SipMove : Ability
{
    protected override void Awake()
    {
        _highlightColor = Color.green;
        base.Awake();
    }
    protected override void OnMouseEnter()
    {
        Description = "Gives 1 more action this turn ! Costs 20% max hp\nCooldown:" + _baseCooldown;
        Targets = new List<Vector2>() { new(0, 0) };
        base.OnMouseEnter();
    }
    public override void PerformAction(Tile tile)
    {
        PlayerData.Instance.ActionsRemaining += 1;
        PlayerData.Instance.TakeDirectDamage((int)(PlayerData.Instance.MaxHealth * 0.2f));

        base.PerformAction(tile);
    }
}
