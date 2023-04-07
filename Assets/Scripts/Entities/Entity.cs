using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;

public abstract class Entity : MonoBehaviour
{

    [SerializeField] private string _name = null;
    [SerializeField] private string _description = null;
    [SerializeField] private int _curentHealth = 0;
    [SerializeField] private int _maxHealth = 0;
    [SerializeField] private int _damage = 0;
    [SerializeField] private int _defense = 0;
    [SerializeField] private int _luck = 0;
    [SerializeField] private int _moveDistance = 0;
    [SerializeField] private int _actionPoints = 0;

    [SerializeField] private SpriteRenderer _deadSR;
    [SerializeField] private Sprite _deadSprite;

    protected List<Vector2> _attackTargets;

    public string Name { get => _name; set => _name = value; }
    public string Description { get => _description; set => _description = value; }
    public bool IsTurn { get; set; }
    public bool IsDead { get; set; }
    public Tile Tile { get; set; }
    public PlayerInBattle Player { get; set; }
    public Dictionary<Vector2, Tile> Grid { get; set; }
    public Sprite Sprite { get; set; }

    public int CurentHealth { get => _curentHealth; set => _curentHealth = value; }
    public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    public int Damage { get => _damage; set => _damage = value; }
    public int Defense { get => _defense; set => _defense = value; }
    public int Luck { get => _luck; set => _luck = value; }
    public int MoveDistance { get => _moveDistance; set => _moveDistance = value; }
    public int ActionPoints { get => _actionPoints; set => _actionPoints = value; }

    protected virtual void Awake()
    {
        GetGrid();
        Tile = GetTile();
        SetAttackTargets();
    }
    protected virtual void SetAttackTargets()
    {
        _attackTargets = new List<Vector2>
        {
            Vector2.down,
            Vector2.up,
            Vector2.right,
            Vector2.left
        };
    }
    public virtual void PerformAction()
    {
        if (IsDead)
        {
            IsTurn = false;
            return;
        }

        if (PlayerInAttackRange())
        {
            Attack();
            StartCoroutine(MoveToPositionThenReturn(transform, Player.transform.position, 0.5f));
        }
        else
            StartCoroutine(FollowPathToPlayer());
    }
    protected IEnumerator FollowPathToPlayer()
    {
        List<Tile> path = AStarSearch(Tile, Player.tile, Grid);
        path.RemoveAt(path.Count - 1);
        for (int i = 0; i < MoveDistance; i++)
        {
            if (i + 1 > path.Count - 1) break;

            path[i + 1].HighLight(Color.blue);
        }

        for (int i = 0; i < MoveDistance; i++)
        {
            if (i + 1 > path.Count - 1) break;

            Vector3 targetPosition = path[i + 1].transform.position;
            Vector3 currentPos = transform.position;
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime / 0.5f;
                transform.position = Vector3.Lerp(currentPos, targetPosition, t);
                yield return null;
            }

            transform.parent = path[i + 1]._entity.transform;
            Tile = GetTile();

        }
        Player.grid.CancelHighlight();
        IsTurn = false;
    }
    public IEnumerator MoveToPositionThenReturn(Transform transform, Vector3 position, float timeToMove)
    {
        foreach (var target in _attackTargets)
        {
            if (Grid.ContainsKey(Tile._position + target))
                Grid[Tile._position + target].HighLight(Color.red);
        }

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

        IsTurn = false;
        Player.grid.CancelHighlight();
    }
    private bool PlayerInAttackRange()
    {
        foreach (var target in _attackTargets)
        {
            if (Grid.ContainsKey(Tile._position + target))
            {
                if (Grid[Tile._position + target].GetComponentInChildren<PlayerInBattle>() != null)
                {
                    return true;
                }
            }
        }

        return false;
    }
    protected void GetGrid()
    {
        Grid = transform.parent.parent.GetComponentInParent<Grid>()._tiles;
    }
    protected Tile GetTile()
    {
        return transform.parent.GetComponentInParent<Tile>();
    }
    public static List<Tile> AStarSearch(Tile start, Tile goal, Dictionary<Vector2, Tile> grid)
    {
        List<Tile> path = new List<Tile>();

        HashSet<Tile> openSet = new HashSet<Tile>();
        HashSet<Tile> closedSet = new HashSet<Tile>();

        Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
        Dictionary<Tile, float> gScore = new Dictionary<Tile, float>();
        Dictionary<Tile, float> fScore = new Dictionary<Tile, float>();

        openSet.Add(start);
        gScore[start] = 0;
        fScore[start] = Heuristic(start, goal);

        while (openSet.Count > 0)
        {
            Tile current = null;
            float lowestFScore = Mathf.Infinity;

            foreach (Tile tile in openSet)
            {
                if (fScore.ContainsKey(tile) && fScore[tile] < lowestFScore)
                {
                    current = tile;
                    lowestFScore = fScore[tile];
                }
            }

            if (current == goal)
            {
                path.Add(current);
                while (cameFrom.ContainsKey(current))
                {
                    current = cameFrom[current];
                    path.Insert(0, current);
                }

                return path;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Tile neighbor in GetNeighbors(current, grid))
            {
                if (closedSet.Contains(neighbor))
                    continue;

                float tentativeGScore = gScore[current] + 1f;
                if (!openSet.Contains(neighbor))
                    openSet.Add(neighbor);
                else if (tentativeGScore >= gScore[neighbor])
                    continue;

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goal);
            }
        }

        return null;
    }
    private static float Heuristic(Tile a, Tile b)
    {
        return Mathf.Abs(a._position.x - b._position.x) + Mathf.Abs(a._position.y - b._position.y);
    }
    private static List<Tile> GetNeighbors(Tile tile, Dictionary<Vector2, Tile> grid)
    {
        List<Tile> neighbors = new List<Tile>();

        foreach (var direction in new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right })
        {
            var neighborPos = tile._position + direction;
            if (grid.ContainsKey(neighborPos))
            {
                var neighbor = grid[neighborPos];
                if (neighbor._entity.GetComponentInChildren<Entity>() == null)
                    neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
    protected virtual void Attack()
    {
        PlayerData.Instance.TakeDamage(Damage);
    }
    public virtual void TakeDamage(int Damage)
    {
        CurentHealth -= Damage - Defense;
        if (CurentHealth <= 0)
        {
            IsDead = true;
            _deadSR.sprite = _deadSprite;
        }
    }
}
