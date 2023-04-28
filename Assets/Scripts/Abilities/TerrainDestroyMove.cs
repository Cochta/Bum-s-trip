using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDestroyMove : Ability
{
    protected override void Awake()
    {
        _highlightColor = Color.red;
        base.Awake();
    }
    protected override void OnMouseEnter()
    {
        Description = "destroy Terrain !";
        Targets = new List<Vector2>();
        Targets.Add(new(1, 1));
        Targets.Add(new(1, 0));
        Targets.Add(new(1, -1));
        Targets.Add(new(-1, -1));
        Targets.Add(new(-1, 0));
        Targets.Add(new(-1, 1));
        Targets.Add(new(0, 1));
        Targets.Add(new(0, -1));

        base.OnMouseEnter();
    }
    public override void PerformAction(Tile tile)
    {
        if (tile.Terrain.GetComponentInChildren<Terrain>() != null)
        {
            StartCoroutine(MoveToPositionThenReturn(_poolManager.Player.transform, tile.transform.position, 0.5f));
            foreach (var terrain in tile.Terrain.GetComponentsInChildren<Terrain>())
            {
                Destroy(terrain.gameObject);
            }
            PlayerData.Instance.ActionsRemaining -= 1;
            RemainingCooldown = _baseCooldown;
        }
        base.PerformAction(tile);
    }
}
