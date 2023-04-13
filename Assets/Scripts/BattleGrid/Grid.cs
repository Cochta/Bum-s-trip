using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;

    public Dictionary<Vector2, Tile> _tiles;

    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Tile tile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                tile.name = $"Tile{x}{y}";
                tile.Position = new Vector2(x, y);
                tile.transform.parent = transform;

                bool isOffset = (x % 2 == 0 && y % 2 != 0) || (y % 2 == 0 && x % 2 != 0);
                tile.Init(isOffset);

                _tiles[new Vector2(x, y)] = tile;
            }
        }
        transform.position = new Vector2(-5, -2.5f);
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
