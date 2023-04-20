using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeMove : Ability
{
    protected override void Awake()
    {
        _highlightColor = Color.red;
        base.Awake();
    }
    protected override void OnMouseEnter()
    {
        Description = "Deal " + PlayerData.Instance.Damage * 2 + " damages ! \nCooldown: " + _baseCooldown;
        Targets = new List<Vector2>();
        Targets.Add(new Vector2(1, 0));
        Targets.Add(new Vector2(-1, 0));
        Targets.Add(new Vector2(0, 1));
        Targets.Add(new Vector2(0, -1));

        base.OnMouseEnter();
    }

    public override void PerformAction(Tile tile)
    {
        if (tile.Entity.GetComponentInChildren<Entity>() != null)
        {
            StartCoroutine(MoveToPositionThenReturn(_poolManager.Player.transform, tile.transform.position, 0.5f));
            tile.Entity.GetComponentInChildren<Entity>().TakeDamage(PlayerData.Instance.Damage * 2);
            PlayerData.Instance.ActionsRemaining -= 1;
            RemainingCooldown = _baseCooldown;
        }
        base.PerformAction(tile);
    }

    public IEnumerator MoveToPositionThenReturn(Transform transform, Vector3 position, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / (timeToMove / 2);
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
        t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / (timeToMove / 2);
            transform.position = Vector3.Lerp(position, currentPos, t);
            yield return null;
        }
        if (PlayerData.Instance.ActionsRemaining <= 0)
        {
            _poolManager.Player.IsPlayerTurn = false;
        }
    }
}
