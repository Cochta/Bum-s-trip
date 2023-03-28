using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Overlays;
using UnityEngine;
using static BattleManager;
using static UnityEngine.EventSystems.EventTrigger;

public class BattleManager : MonoBehaviour
{
    public enum GameStates
    {
        Initialisation,
        PlayerTurn,
        EnemiesTurn,
        None
    }

    private Grid _grid;

    public GameObject PlayerPrefab;
    public EnemyLayout EnemyLayout;

    public List<GameObject> Enemies;
    public PlayerInBattle Player;

    public GameStates GameState = GameStates.None;

    [SerializeField] private Stuff _stuff;
    public MovePoolManager Pool;

    void Start()
    {
        ChangeState(GameStates.Initialisation);
    }
    private void Update()
    {
        if (GameState == GameStates.PlayerTurn)
        {
            if (!Player.IsPlayerTurn)
            {
                PlayerData.Instance.UpdateData();
                ChangeState(GameStates.EnemiesTurn);
            }
        }
        else if (GameState == GameStates.EnemiesTurn)
        {
            // Check if there are any enemies left to play
            int enemiesLeftToPlay = 0;
            foreach (var enemy in Enemies)
            {
                if (enemy.GetComponent<Entity>().IsTurn && !enemy.GetComponent<Entity>().IsDead)
                {
                    enemiesLeftToPlay++;
                }
            }
            if (enemiesLeftToPlay == 0)
            {
                // If there are no enemies left to play, change state to PlayerTurn
                ChangeState(GameStates.PlayerTurn);
            }
        }
    }

    public void SpawnPlayer()
    {
        Player = Instantiate(PlayerPrefab, _grid.GetTile(5, 1)._entity.transform).GetComponent<PlayerInBattle>();
        Pool.Player = Player;
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < EnemyLayout.Enemies.Count; i++)
        {
            Enemies.Add(Instantiate(EnemyLayout.Enemies[i], _grid.GetTile(EnemyLayout.Positions[i])._entity.transform));
            Enemies[i].GetComponent<Entity>().Player = Player;
        }
    }
    public void ChangeState(GameStates newState)
    {
        GameState = newState;

        switch (newState)
        {
            case GameStates.Initialisation:
                _grid = GetComponent<Grid>();
                _grid.GenerateGrid();
                SpawnPlayer();
                SpawnEnemies();
                ChangeState(GameStates.PlayerTurn);
                break;
            case GameStates.PlayerTurn:
                Player.IsPlayerTurn = true;
                break;
            case GameStates.EnemiesTurn:
                Player.IsPlayerTurn = false;
                StartCoroutine(EnemyTurnCoroutine());
                break;
            case GameStates.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    private IEnumerator EnemyTurnCoroutine()
    {
        foreach (var enemy in Enemies)
        {
            Entity entity = enemy.GetComponent<Entity>();
            entity.IsTurn = true;
            entity.PerformAction();

            yield return new WaitUntil(() => !entity.IsTurn);

            yield return new WaitForSeconds(0.5f);
        }
    }
}
