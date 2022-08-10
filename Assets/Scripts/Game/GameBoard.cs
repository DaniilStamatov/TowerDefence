using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private Transform _board;
    [SerializeField] private GameTile _tilePrefab;

    private Vector2Int _size;

    private GameTile[] _tiles;

    private Queue<GameTile> _searchBorders = new Queue<GameTile>();
    private GameTileContentFactory _contentFactory;

    private List<GameTile> _spawnPoints = new List<GameTile>();
    public int SpawnPointCount => _spawnPoints.Count;
    private List<GameTileContent> _contentToUpdate = new List<GameTileContent> ();

    public void Initialize(Vector2Int size, GameTileContentFactory factory)
    {
        _size = size;
        _board.localScale = new Vector3(size.x, size.y, 1f);

        var offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);
        _contentFactory = factory;
        //инициализация на нужное количество элементов
        _tiles = new GameTile[size.x * size.y];

        for (int i = 0, y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                var tile = _tiles[i] = Instantiate(_tilePrefab);
                tile.transform.SetParent(transform, false);
                tile.transform.localPosition = new Vector3(x - offset.x, 0.01f, y - offset.y);

                if (x > 0)
                {
                    GameTile.MakeEastWestNeighbours(tile, _tiles[i - 1]);
                }

                if (y > 0)
                {
                    GameTile.MakeNorthSouthNeighbours(tile, _tiles[i - size.x]);
                }

                //x&1 => x/2
                tile.IsAlternative = (x & 1) == 0;
                if ((y & 1) == 0)
                {
                    tile.IsAlternative = !tile.IsAlternative;
                }
                tile.Content = factory.Get(GameTileContentType.Empty);
            }
        }

        Clear();
    }

    public void GameUpdate()
    {
        for (int i = 0; i < _contentToUpdate.Count; i++)
        {
            _contentToUpdate[i].GameUpdate();
        }
    }

    private bool FindPath()
    {

        foreach (var tile in _tiles)
        {

            if (tile.Content.Type == GameTileContentType.Destination)
            {
                tile.BecomeDestination();
                _searchBorders.Enqueue(tile);
            }
            else
            {
                tile.ClearPath();
            }
        }

        if (_searchBorders.Count == 0)
        {
            return false;
        }


        while (_searchBorders.Count > 0)
        {
            GameTile tile = _searchBorders.Dequeue();

            if (tile != null)
            {
                if (tile.IsAlternative)
                {

                    _searchBorders.Enqueue(tile.GrowPathNorth());
                    _searchBorders.Enqueue(tile.GrowPathSouth());
                    _searchBorders.Enqueue(tile.GrowPathEast());
                    _searchBorders.Enqueue(tile.GrowPathWest());
                }
                else
                {
                    _searchBorders.Enqueue(tile.GrowPathWest());
                    _searchBorders.Enqueue(tile.GrowPathEast());
                    _searchBorders.Enqueue(tile.GrowPathSouth());
                    _searchBorders.Enqueue(tile.GrowPathNorth());
                }
            }
        }

        foreach (var tile in _tiles)
        {
            if (!tile.HasPath)
            {
                return false;
            }
        }

        foreach (var tile in _tiles)
        {
            tile.ShowPath();
        }

        return true;
    }


    public void ToggleDestination(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Destination)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            if (!FindPath())
            {
                tile.Content = _contentFactory.Get(GameTileContentType.Destination);
                FindPath();

            }
            FindPath();
        }
        else if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Destination);
            FindPath();
        }
    }

    public void ToggleWall(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.Wall)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            FindPath();
        }

        else if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Wall);
            if (!FindPath())
            {
                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
                FindPath();
            }
            FindPath();
        }
    }

    public void ToggleSpawnPoint(GameTile tile)
    {
        if (tile.Content.Type == GameTileContentType.SpawnPoint)
        {
            if (_spawnPoints.Count > 1)
            {
                _spawnPoints.Remove(tile);
                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            }
        }

        else if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.SpawnPoint);
            _spawnPoints.Add(tile);
        }
    }

    public void ToggleTower(GameTile tile, TowerType towerType)
    {
        if (tile.Content.Type == GameTileContentType.Tower)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
            FindPath();
            _contentToUpdate.Remove(tile.Content);
        }

        else if (tile.Content.Type == GameTileContentType.Empty)
        {
            tile.Content = _contentFactory.Get(towerType);
            if (FindPath())
            {
                _contentToUpdate.Add(tile.Content);
            }
            else
            {
                tile.Content = _contentFactory.Get(GameTileContentType.Empty);
                FindPath();
            }
        }

        else if (tile.Content.Type == GameTileContentType.Wall)
        {
            tile.Content = _contentFactory.Get(towerType);
            _contentToUpdate.Add(tile.Content);
        }
    }


    public GameTile GetTile(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, 1))
        {
            int x = (int)(hit.point.x + _size.x * 0.5f);
            int y = (int)(hit.point.z + _size.y * 0.5f);

            if (x >= 0 && x < _size.x && y >= 0 && y < _size.y)
            {
                return _tiles[x + y * _size.x];
            }
        }
        return null;
    }

    public GameTile GetSpawnPoint(int index)
    {
        return _spawnPoints[index];
    }

    public void Clear()
    {
        foreach (var tile in _tiles)
        {
            tile.Content = _contentFactory.Get(GameTileContentType.Empty);
        }
        _spawnPoints.Clear();
        _contentToUpdate.Clear();
        ToggleDestination(_tiles[_tiles.Length / 2]);
        ToggleSpawnPoint(_tiles[0]);
    }
}
