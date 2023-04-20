using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpRat : RandomEvent
{
    [SerializeField] private Item _reward;
    public override void GoodEnding()
    {
        var loot = PlayerData.Instance.Loot;
        loot.SpecificItem = _reward;
        loot.gameObject.SetActive(true);
        loot.GenerateLoot();
        base.GoodEnding();
    }
    public override void BadEnding()
    {
        PlayerData.Instance.TakeDirectDamage(1);
        base.BadEnding();
    }
}
