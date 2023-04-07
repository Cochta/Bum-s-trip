using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public enum GameStates
    {
        Initialisation,
        StartBattle,
        PlayerTurn,
        EnemiesTurn,
        None
    }

    private Grid _grid;

    public GameObject PlayerPrefab;

    public List<GameObject> Enemies;
    public PlayerInBattle Player;

    public GameStates GameState = GameStates.None;

    [SerializeField] private Stuff _stuff;
    public MovePoolManager Pool;

    [SerializeField] private EnemyPool _enemyPool;

    private bool IsBattle = false;

    private void Update()
    {
        if (!IsBattle)
        {
            return;
        }

        int deadEnemies = 0;
        foreach (var enemy in Enemies)
        {
            if (enemy.GetComponent<Entity>().IsDead)
            {
                deadEnemies++;
            }
        }

        if (deadEnemies == Enemies.Count)
        {
            EndBattle();
            return;
        }

        if (GameState == GameStates.PlayerTurn && !Player.IsPlayerTurn)
        {
            PlayerData.Instance.UpdateData();
            ChangeState(GameStates.EnemiesTurn);
        }
        else if (GameState == GameStates.EnemiesTurn)
        {
            int enemiesLeftToPlay = Enemies.Count(e => e.GetComponent<Entity>().IsTurn && !e.GetComponent<Entity>().IsDead);
            if (enemiesLeftToPlay == 0)
            {
                ChangeState(GameStates.PlayerTurn);
            }
        }
    }

    public void EndBattle() //normally private but for generator tests public
    {
        PlayerData.Instance.ChangeGameState(PlayerData.GameStates.ToTreasure);
        foreach (var enemy in Enemies)
        {
            Destroy(enemy);
        }
        IsBattle = false;
        gameObject.GetComponent<EntityDisplay>().Stats.SetActive(false);
        Pool.ClearMovepool();
    }

    public void SpawnPlayer()
    {
        Player = Instantiate(PlayerPrefab, _grid.GetTile(5, 1)._entity.transform).GetComponent<PlayerInBattle>();
        Pool.Player = Player;
    }

    private void ResetPlayer()
    {
        Player.tile = _grid.GetTile(5, 1);
        Player.transform.parent = Player.tile.transform;
        Player.transform.position = Player.transform.parent.position;
    }
    public void SpawnEnemies()
    {
        System.Random rnd = new System.Random();
        EnemyLayout layout = _enemyPool.PoolCave[rnd.Next(0, _enemyPool.PoolCave.Count())];
        Enemies = new List<GameObject>();
        for (int i = 0; i < layout.Enemies.Count; i++)
        {
            Enemies.Add(Instantiate(layout.Enemies[i], _grid.GetTile(layout.Positions[i])._entity.transform));
            Enemies[i].GetComponent<Entity>().Player = Player;
        }
    }
    public void ChangeState(GameStates newState)
    {
        PlayerData.Instance.UpdateData();
        GameState = newState;

        switch (newState)
        {
            case GameStates.Initialisation:
                _grid = GetComponent<Grid>();
                _grid.GenerateGrid();
                SpawnPlayer();
                break;
            case GameStates.StartBattle:
                ResetPlayer();
                SpawnEnemies();
                IsBattle = true;
                ChangeState(GameStates.PlayerTurn);
                break;
            case GameStates.PlayerTurn:
                PlayerData.Instance.DecrementCooldown();
                PlayerData.Instance.EnableAbilities();
                Player.IsPlayerTurn = true;
                break;
            case GameStates.EnemiesTurn:
                PlayerData.Instance.DisableAbilities();
                Player.IsPlayerTurn = false;
                StopCoroutine(EnemyTurnCoroutine());
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
            enemy.GetComponent<Entity>().IsTurn = true;
        }
        foreach (var enemy in Enemies)
        {
            Entity entity = enemy.GetComponent<Entity>();
            StartCoroutine(entity.PerformAction());

            yield return new WaitUntil(() => !entity.IsTurn);

            yield return new WaitForSeconds(0.5f);
        }
    }
}
