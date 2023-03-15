using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Overlays;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class BattleManager : MonoBehaviour
{
    public enum GameStates
    {
        GenerateGrid,
        SpawnPlayer,
        SpawnEnemies,
        PlayerTurn,
        EnemiesTurn,
        None
    }

    private Grid _grid;

    public GameObject PlayerPrefab;
    public EnemyLayout EnemyLayout;

    public List<GameObject> Enemies;
    public GameObject Player;

    public GameStates GameState = GameStates.None;

    void Start()
    {
        _grid = GetComponent<Grid>();
        ChangeState(GameStates.GenerateGrid);
        ChangeState(GameStates.SpawnPlayer);
        ChangeState(GameStates.SpawnEnemies);
        Battle();
    }

    public void Battle()
    {
        ChangeState(GameStates.PlayerTurn);
        ChangeState(GameStates.EnemiesTurn);
    }

    public void SpawnPlayer()
    {
        Player = Instantiate(PlayerPrefab, _grid.GetTile(2, 3)._entity.transform);
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
            case GameStates.GenerateGrid:
                _grid.GenerateGrid();
                break;
            case GameStates.SpawnPlayer:
                SpawnPlayer();
                break;
            case GameStates.SpawnEnemies:
                SpawnEnemies();
                break;
            case GameStates.PlayerTurn:
                //PlayerPrefab.taper
                Debug.Log("player taper");
                break;
            case GameStates.EnemiesTurn:
                foreach (var enemy in Enemies)
                {

                    enemy.GetComponent<Entity>().PerformAction();
                }
                break;
            case GameStates.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }
}
