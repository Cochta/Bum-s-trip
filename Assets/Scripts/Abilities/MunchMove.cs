using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MunchMove : Ability
{
    protected override void Awake()
    {
        _highlightColor = Color.green;
        base.Awake();
    }
    protected override void OnMouseEnter()
    {
        Description = "50% chance to heal 20% max HP 50% chance to lose 20% max HP\nCooldown:" + _baseCooldown;
        Targets = new List<Vector2>() { new(0, 0) };
        base.OnMouseEnter();
    }
    public override void PerformAction(Tile tile)
    {
        PlayerData.Instance.ActionsRemaining -= 1;
        int rnd = Random.Range(0, 101);
        if (rnd <= 50 + PlayerData.Instance.Luck)
            PlayerData.Instance.Heal((int)(PlayerData.Instance.MaxHealth * 0.2f));
        else
            PlayerData.Instance.TakeDirectDamage((int)(PlayerData.Instance.MaxHealth * 0.2f));

        RemainingCooldown = _baseCooldown;
        base.PerformAction(tile);
    }
}
