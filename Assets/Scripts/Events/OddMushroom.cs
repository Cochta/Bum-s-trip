using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OddMushroom : RandomEvent
{
    public override void GoodEnding()
    {
        PlayerData.Instance.Heal((int)(PlayerData.Instance.MaxHealth * 0.2f));
        base.GoodEnding();
    }
    public override void BadEnding()
    {
        PlayerData.Instance.TakeDirectDamage((int)(PlayerData.Instance.MaxHealth * 0.2f));
        base.BadEnding();
    }
}
