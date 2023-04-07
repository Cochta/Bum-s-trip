using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceMove : Ability
{
    protected override void Awake()
    {
        _highlightColor = Color.red;
        base.Awake();
    }
    protected override void OnMouseEnter()
    {
        Targets = new List<Vector2>();
        Targets.Add(new Vector2(1, 0));
        Targets.Add(new Vector2(-1, 0));
        Targets.Add(new Vector2(0, 1));
        Targets.Add(new Vector2(0, -1));

        base.OnMouseEnter();
    }

    public override void PerformAction(Tile tile)
    {
        if (tile._entity.GetComponentInChildren<Entity>() != null)
        {
            StartCoroutine(MoveToPositionThenReturn(_poolManager.Player.transform, tile.transform.position, 0.5f));
            tile._entity.GetComponentInChildren<Entity>().TakeDamage(PlayerData.Instance.Damage);
            PlayerData.Instance.ActionPoints -= 1;
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
        if (PlayerData.Instance.ActionPoints <= 0)
        {
            _poolManager.Player.IsPlayerTurn = false;
        }
    }
}
