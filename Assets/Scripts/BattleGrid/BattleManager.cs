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
        EndGame,
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
        Battle();
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
        int enemytoplay = 0;
        if (GameState == GameStates.EnemiesTurn)
        {
            foreach (var enemy in Enemies)
            {
                if (!enemy.GetComponent<Entity>().IsTurn || enemy.GetComponent<Entity>().Isdead)
                    enemytoplay++;
            }
            if (enemytoplay == Enemies.Count)
            {
                ChangeState(GameStates.PlayerTurn);
            }
        }
    }


    public void Battle()
    {
        ChangeState(GameStates.PlayerTurn);
    }

    public void SpawnPlayer()
    {
        Player = Instantiate(PlayerPrefab, _grid.GetTile(2, 1)._entity.transform).GetComponent<PlayerInBattle>();
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
                break;
            case GameStates.PlayerTurn:
                Player.IsPlayerTurn = true;
                EnableAbilities();
                break;
            case GameStates.EnemiesTurn:
                DisableAbilities();
                StartCoroutine(EnemiesTurn());
                break;
            case GameStates.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
    private IEnumerator EnemiesTurn()
    {
        foreach (var enemy in Enemies)
        {
            enemy.GetComponent<Entity>().IsTurn = true;
            enemy.GetComponent<Entity>().PerformAction();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void EnableAbilities()
    {
        foreach (var ability in Pool.Abilities)
        {
            ability._col.enabled = true;
        }
    }
    private void DisableAbilities()
    {
        foreach (var ability in Pool.Abilities)
        {
            ability._col.enabled = false;
            ability.IsSelected = false;
        }
    }
}
