using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.GraphicsBuffer;

public abstract class Entity : MonoBehaviour
{
    public int CurentHealth = 0;
    public int MaxHealth = 0;
    public int Damage = 0;
    public int Defense = 0;
    public int Luck = 0;
    public int MoveDistance = 0; // nb of tiles it can move
    public int ActionPoints = 0;

    public Tile tile;
    public PlayerInBattle Player;
    public Dictionary<Vector2, Tile> grid;

    public string Name = null;

    public Sprite _sprite;

    public bool IsDead = false;

    [SerializeField] private SpriteRenderer _deadSR;
    [SerializeField] private Sprite _deadSprite;

    public bool IsTurn = false;

    protected List<Vector2> attackTargets;
    public bool HasFinishedTurn = false;

    protected virtual void Awake()
    {
        GetGrid();
        tile = GetTile();
        SetAttackTargets();
    }

    protected virtual void SetAttackTargets()
    {
        attackTargets = new List<Vector2>
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
        List<Tile> path = AStarSearch(tile, Player.tile, grid);
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
            tile = GetTile();

        }
        Player.grid.CancelHighlight();
        IsTurn = false;
    }

    //protected IEnumerator MoveToPlayer()
    //{
    //    List<Tile> path = AStarSearch(tile, Player.tile, grid);
    //    path.RemoveAt(path.Count - 1);
    //    for (int i = 0; i < MoveDistance; i++)
    //    {
    //        if (i + 1 > path.Count - 1) break;

    //        path[i + 1].HighLight(Color.blue);

    //    }

    //    for (int i = 0; i < MoveDistance; i++)
    //    {
    //        if (i + 1 > path.Count - 1) break;

    //        Vector3 targetPosition = path[i + 1].transform.position;
    //        transform.parent = path[i + 1]._entity.transform;
    //        yield return StartCoroutine(MoveToPosition(targetPosition, 0.5f));

    //        tile = GetTile();
    //    }

    //}

    //public IEnumerator MoveToPosition(Vector3 position, float timeToMove) // doit changer en follow path
    //{
    //    Vector3 currentPos = transform.position;
    //    float t = 0f;

    //    while (t < 1f)
    //    {
    //        t += Time.deltaTime / timeToMove;
    //        transform.position = Vector3.Lerp(currentPos, position, t);
    //        yield return null;
    //    }

    //    IsTurn = false;
    //    Player.grid.CancelHighlight();
    //}

    public IEnumerator MoveToPositionThenReturn(Transform transform, Vector3 position, float timeToMove)
    {
        foreach (var target in attackTargets)
        {
            if (grid.ContainsKey(tile._position + target))
                grid[tile._position + target].HighLight(Color.red);
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
        foreach (var target in attackTargets)
        {
            if (grid.ContainsKey(tile._position + target))
            {
                if (grid[tile._position + target].GetComponentInChildren<PlayerInBattle>() != null)
                {
                    return true;
                }
            }
        }

        return false;
    }

    protected void GetGrid()
    {
        grid = transform.parent.parent.GetComponentInParent<Grid>()._tiles;
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
