using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;

    public Dictionary<Vector2, Tile> _tiles;

    [SerializeField] private GridPool _pool;

    private GameObject _grid;

    //public void GenerateGrid()
    //{
    //    _tiles = new Dictionary<Vector2, Tile>();
    //    for (int x = 0; x < _width; x++)
    //    {
    //        for (int y = 0; y < _height; y++)
    //        {
    //            Tile tile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
    //            tile.name = $"Tile{x}{y}";
    //            tile.Position = new Vector2(x, y);
    //            tile.transform.parent = transform;

    //            bool isOffset = (x % 2 == 0 && y % 2 != 0) || (y % 2 == 0 && x % 2 != 0);
    //            tile.Init(isOffset);

    //            _tiles[new Vector2(x, y)] = tile;
    //            tile.ED = GetComponent<EntityDisplay>();
    //            tile.BM = GetComponent<BattleManager>();
    //        }
    //    }
    //    transform.position = new Vector2(-5, -2.5f);
    //}
    //public void GenerateGrid()
    //{
    //    _tiles = new Dictionary<Vector2, Tile>();

    //    GameObject grid = _pool.Grids[Random.Range(0, _pool.Grids.Count)];

    //    foreach (var tile in grid.GetComponentsInChildren<Tile>())
    //    {
    //        Instantiate(tile, transform);
    //        _tiles[new(tile.Position.x, tile.Position.y)] = tile;   
    //    }
    //}
    public void GenerateGrid()
    {
        if (_grid != null)
        {
            Destroy(_grid);
        }
        _tiles = new Dictionary<Vector2, Tile>();

        _grid = Instantiate(_pool.Grids[Random.Range(0, _pool.Grids.Count)], transform);

        foreach (var tile in _grid.GetComponentsInChildren<Tile>())
        {
            tile.Init();
            _tiles[new(tile.Position.x, tile.Position.y)] = tile;
            tile.ED = GetComponent<EntityDisplay>();
            tile.BM = GetComponent<BattleManager>();
        }
    }

    public Tile GetTile(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile))
        {
            return tile;
        }
        return null;
    }
    public Tile GetTile(int x, int y)
    {
        return GetTile(new Vector2(x, y));
    }
    public bool HasTile(Vector2 pos) { return _tiles.ContainsKey(pos); }

    public void CancelHighlight()
    {
        foreach (var tile in _tiles)
        {
            tile.Value.CanceHighlight();
        }
    }
}
