using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bat : Entity
{
    protected override void Awake()
    {
        GetGrid();
        tile = GetTile();
        SetAttackTargets();
    }

    protected override void SetAttackTargets()
    {
        base.SetAttackTargets();
        Debug.Log("penis");
        var targets = attackTargets;
        var newList = new List<Vector2>();
        foreach (var target1 in targets)
        {
            foreach (var target2 in targets)
            {
                if (!newList.Contains(target1 + target2))
                    newList.Add(target1 + target2);
            }
        }

        newList.Remove(new(2, 2));
        newList.Remove(new(2, -2));
        newList.Remove(new(-2, 2));
        newList.Remove(new(-2, -2));

        attackTargets.AddRange(newList);

    }
}
