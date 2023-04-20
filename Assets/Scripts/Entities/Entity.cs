using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    [SerializeField] private int _baseActionPoints = 0;
    protected int _currentActionPoints;

    [SerializeField] private SpriteRenderer _deadSR;
    [SerializeField] private Sprite _deadSprite;

    protected List<Vector2> _attackTargets;

    [SerializeField] protected int _attackRange;

    public List<GameObject> SpawnedEntities = new List<GameObject>();

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
    public int BaseActionPoints { get => _baseActionPoints; set => _baseActionPoints = value; }

    protected virtual void Awake()
    {
        GetGrid();
        Tile = GetTile();
    }
    public virtual IEnumerator PerformAction()
    {
        _attackTargets = SetTargets(_attackRange);
        _currentActionPoints = BaseActionPoints;

        if (IsDead)
        {
            IsTurn = false;
        }
        else
        {
            for (int i = 0; i < BaseActionPoints; i++)
            {
                int wait = _currentActionPoints;
                if (PlayerInAttackRange())
                {
                    Attack();
                    StartCoroutine(MoveToPositionThenReturn(transform, Player.transform.position, 0.5f));

                }
                else
                    StartCoroutine(FollowPath(AStarSearch(Tile, Player.tile, Grid)));

                yield return new WaitUntil(() => _currentActionPoints == wait - 1);
            }
        }
    }
    protected IEnumerator FollowPath(List<Tile> path)
    {
        if (path == null)
        {
            EndAction();
            yield return null;
        }
        if (Player.tile.Position.x == path[path.Count - 1].Position.x && Player.tile.Position.y == path[path.Count - 1].Position.y)
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

            transform.parent = path[i + 1].Entity.transform;
            Tile = GetTile();

        }
        EndAction();
    }
    public IEnumerator MoveToPositionThenReturn(Transform transform, Vector3 position, float timeToMove)
    {
        foreach (var target in _attackTargets)
        {
            if (Grid.ContainsKey(Tile.Position + target))
                Grid[Tile.Position + target].HighLight(Color.red);
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

        EndAction();
    }
    protected bool PlayerInAttackRange()
    {
        foreach (var target in _attackTargets)
        {
            if (Grid.ContainsKey(Tile.Position + target))
            {
                if (Grid[Tile.Position + target].GetComponentInChildren<PlayerInBattle>() != null)
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
                foreach (var p in path)
                {
                    p.HighLight(Color.magenta);
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
        return Mathf.Abs(a.Position.x - b.Position.x) + Mathf.Abs(a.Position.y - b.Position.y);
    }
    private static List<Tile> GetNeighbors(Tile tile, Dictionary<Vector2, Tile> grid)
    {
        List<Tile> neighbors = new List<Tile>();

        foreach (var direction in new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right })
        {
            var neighborPos = tile.Position + direction;
            if (grid.ContainsKey(neighborPos))
            {
                var neighbor = grid[neighborPos];

                bool canWalk = true;
                if (neighbor.Terrain.GetComponentInChildren<Terrain>() != null)
                {
                    if (!neighbor.Terrain.GetComponentInChildren<Terrain>().IsWalkable)
                    {
                        canWalk = false;
                    }
                }
                if (neighbor.Entity.GetComponentInChildren<Entity>() == null && canWalk)
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
        if (Damage > Defense)
            CurentHealth -= Damage - Defense;
        else
            CurentHealth -= 1;
        if (CurentHealth <= 0)
        {
            IsDead = true;
            _deadSR.sprite = _deadSprite;
        }
    }
    protected void EndAction()
    {
        Player.grid.CancelHighlight();
        _currentActionPoints--;
        if (_currentActionPoints <= 0)
            IsTurn = false;
    }
    public static List<Tile> Flee(Tile start, Tile danger, Dictionary<Vector2, Tile> grid)
    {
        Vector2 startVec = start.Position;
        Vector2 dangerVec = danger.Position;
        Vector2 fleeDirection = startVec - dangerVec;
        fleeDirection.Normalize();

        Tile targetTile = null;
        float maxDistance = 0;

        foreach (Tile tile in grid.Values)
        {
            bool walkable = true;
            // Check if the tile is reachable
            if (tile.Terrain.GetComponentInChildren<Terrain>() != null)
            {
                if (!tile.Terrain.GetComponentInChildren<Terrain>().IsWalkable)
                {
                    walkable = false;
                }
            }
            if (tile.Entity.GetComponentInChildren<Entity>() == null && walkable)
            {
                Vector2 tileVec = tile.Position;
                float distance = Vector2.Dot(tileVec - dangerVec, fleeDirection);

                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    targetTile = tile;
                }
            }
        }
        return AStarSearch(start, targetTile, grid);
    }
    protected IEnumerable<Vector2> GetNeighborTiles(Vector2 tile)
    {
        yield return tile + Vector2.up;
        yield return tile + Vector2.down;
        yield return tile + Vector2.left;
        yield return tile + Vector2.right;
    }
    protected IEnumerable<Vector2> GetTilesInRange(IEnumerable<Vector2> tiles, Vector2 center, int range)
    {
        foreach (var tile in tiles)
        {
            if (Vector2.Distance(tile, center) <= range)
            {
                yield return tile;
            }
        }
    }
    protected virtual List<Vector2> SetTargets(int range)
    {
        List<Vector2> targets = new List<Vector2>();
        var pos = Tile.Position;

        var visited = new HashSet<Vector2>();
        var queue = new Queue<Vector2>();
        var distances = new Dictionary<Vector2, int> { { pos, 0 } };

        queue.Enqueue(pos);
        visited.Add(pos);

        while (queue.Count > 0)
        {
            var currentTile = queue.Dequeue();
            var currentDistance = distances[currentTile];

            foreach (var neighborTile in GetNeighborTiles(currentTile))
            {
                if (visited.Contains(neighborTile)) continue;
                var neighborDistance = currentDistance + 1;

                if (neighborDistance > range) break;

                visited.Add(neighborTile);
                distances[neighborTile] = neighborDistance;
                queue.Enqueue(neighborTile);
            }
        }

        foreach (var targetTile in GetTilesInRange(distances.Keys, pos, range))
        {
            targets.Add(targetTile - pos);
        }
        return targets;
    }
}
