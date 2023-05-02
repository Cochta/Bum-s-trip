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
    [SerializeField] private GameObject _rockPrefab;

    public List<GameObject> Enemies;
    private List<GameObject> Terrains = new List<GameObject>();
    public PlayerInBattle Player;

    public GameStates GameState = GameStates.None;

    [SerializeField] private Stuff _stuff;
    public MovePoolManager Pool;

    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private TerrainPool _terrainPool;

    private bool IsBattle = false;

    private int _money = 0;

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
        foreach (var enemy in Enemies)
        {
            foreach (var entity in enemy.GetComponent<Entity>().SpawnedEntities)
            {
                Destroy(entity);
            }
            Destroy(enemy);
        }
        if (Terrains.Count > 0)
        {
            foreach (var terrain in Terrains)
            {
                Destroy(terrain);
            }
        }
        Terrains = new List<GameObject>();

        IsBattle = false;
        gameObject.GetComponent<EntityDisplay>().Stats.SetActive(false);
        Pool.ClearMovepool();
        if (PlayerData.Instance.State == PlayerData.GameStates.ToBoss)
        {
            PlayerData.Instance.GainMoney(Random.Range(10, 21));
            PlayerData.Instance.ChangeGameState(PlayerData.GameStates.ToBossLoot);
        }
        else
        {
            PlayerData.Instance.GainMoney(Random.Range(_money / 3, _money + 1));
            PlayerData.Instance.ChangeGameState(PlayerData.GameStates.ToTreasure);
        }
        _money = 0;
        PlayerData.Instance.UpdateData();

    }

    public void SpawnPlayer()
    {
        Player = Instantiate(PlayerPrefab, _grid.GetTile(5, 1).Entity.transform).GetComponent<PlayerInBattle>();
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
        Enemies = new List<GameObject>();
        EnemyLayout layout;
        if (PlayerData.Instance.State == PlayerData.GameStates.ToBoss)
        {
            layout = _enemyPool.Layouts[0].layouts[0];
        }
        else
        {
            ListWrapper wrapper = _enemyPool.Layouts[Random.Range(0, _enemyPool.Layouts.Count)];
            layout = wrapper.layouts[Random.Range(0, _enemyPool.Layouts.Count - 1)];
        }
        foreach (var pos in layout.Positions)
        {
            List<GameObject> pool;
            if (PlayerData.Instance.State == PlayerData.GameStates.ToBoss)
            {
                pool = new List<GameObject>(_enemyPool.PoolCaveBoss);
                Enemies.Add(Instantiate(pool[Random.Range(0, pool.Count)], _grid.GetTile(pos).Entity.transform));
            }
            else
            {
                pool = new List<GameObject>(_enemyPool.PoolCave);
                Enemies.Add(Instantiate(pool[Random.Range(0, pool.Count)], _grid.GetTile(pos).Entity.transform));
            }
            Enemies.Last().GetComponent<Entity>().Player = Player;
            _money += 3;
        }
    }
    public void SpawnTerrain()
    {
        Terrains = new List<GameObject>();
        System.Random rnd = new System.Random();
        TerrainLayout borderLayout = _terrainPool.Borders[rnd.Next(0, _terrainPool.Borders.Count())];
        for (int i = 0; i < borderLayout.Positions.Count; i++)
        {
            Terrains.Add(Instantiate(_rockPrefab, _grid.GetTile(borderLayout.Positions[i]).Terrain.transform));
        }
        TerrainLayout centerLayout = _terrainPool.Centers[rnd.Next(0, _terrainPool.Centers.Count())];
        for (int i = 0; i < centerLayout.Positions.Count; i++)
        {
            Terrains.Add(Instantiate(_rockPrefab, _grid.GetTile(centerLayout.Positions[i]).Terrain.transform));
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
                SpawnTerrain();
                IsBattle = true;
                ChangeState(GameStates.PlayerTurn);
                break;
            case GameStates.PlayerTurn:
                PlayerData.Instance.ResetActions();
                PlayerData.Instance.UpdateData();
                PlayerData.Instance.DecrementCooldown();
                PlayerData.Instance.EnableAbilities();
                Player.IsPlayerTurn = true;
                break;
            case GameStates.EnemiesTurn:
                PlayerData.Instance.DisableAbilities();
                Player.IsPlayerTurn = false;
                StopCoroutine("EnemyTurnCoroutine");
                StartCoroutine("EnemyTurnCoroutine");
                break;
            case GameStates.None:
                break;
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
