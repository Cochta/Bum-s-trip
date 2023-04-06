using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LootButton : MonoBehaviour
{
    private SpriteRenderer _sr;

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        PlayerData.Instance.ChangeGameState(PlayerData.GameStates.ToMap);
        OnMouseExit();
    }

    private void OnMouseEnter()
    {
        _sr.color = Color.cyan;
    }

    private void OnMouseExit()
    {
        _sr.color = Color.white;
    }
}
