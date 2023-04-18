using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGenerator : MonoBehaviour
{
    private SpriteRenderer _sr;
    [SerializeField] private BattleManager _bm;

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        PlayerData.Instance.ChangeGameState(PlayerData.GameStates.ToBattle);
    }

    private void OnMouseDown()
    {
        _bm.EndBattle();
        PlayerData.Instance.ChangeGameState(PlayerData.GameStates.ToBattle);
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
