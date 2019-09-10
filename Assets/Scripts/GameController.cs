using System.Collections.Generic;
using Entities;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public GameConstants GameConstants;
    
    public Transform MapTransform;
    
    public Ball BallPrefab;
    public Tile TilePrefab;
    public Gem GemPrefab;

    private GemLogic _gemLogic;

    private Ball _ball;
    public Ball Ball => _ball;

    private List<Tile> _tiles;
    private int _lastTileX;
    private int _lastTileY;
    
    private List<Gem> _gems;
    private HashSet<uint> _hashes;

    private int _score;
    public int Score => _score;

    private bool _gameStarted;
    public bool GameStarted => _gameStarted;

    private void Awake()
    {
        PoolingObject<Ball>.Init(BallPrefab, MapTransform, GameConstants.BallsPoolSize);
        PoolingObject<Tile>.Init(TilePrefab, MapTransform, GameConstants.TilesPoolSize);
        PoolingObject<Gem>.Init(GemPrefab, MapTransform, GameConstants.GemsPoolSize);
        
        _tiles = new List<Tile>();
        _gems = new List<Gem>();
        _hashes = new HashSet<uint>();
        
        _gemLogic = new GemLogic();

        NewGame();
    }

    private void NewGame()
    {
        ClearAll();
        CreateMap();
        CreateBall();
        _gameStarted = true;
        _score = 0;
    }

    private void ClearAll()
    {
        foreach (var tile in _tiles)
        {
            tile.Release();
        }
        _tiles.Clear();
        
        foreach (var gem in _gems)
        {
            gem.Release();
        }
        _gems.Clear();
        
        _hashes.Clear();

        if (_ball)
        {
            _ball.Release();
            _ball = null;
        }
    }

    private void CreateBall()
    {
        _ball = PoolingObject<Ball>.GetNew();
        _ball.MoveComponent.Speed = GameConstants.BallSpeed;
    }

    private void CreateMap()
    {
        var size = GameConstants.StartingPlatformSize;
        _gemLogic.Init(- size * size, GameConstants.GemGenerationBlock, GameConstants.RandomGems);
        _lastTileX = size - 1;
        _lastTileY = size - 1;
        CreateBlock(_lastTileX, _lastTileY, size, size);
        
        FillField();
    }
    
    private void CreateBlock(int x, int y, int width, int height)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                CreateTile(x - i, y - j);
            }
        }
    }

    private void CreateTile(int x, int y)
    {
        var tile = PoolingObject<Tile>.GetNew();
        tile.X = x;
        tile.Y = y;
        tile.transform.localPosition = new Vector3(x, 0, y);
        _tiles.Add(tile);
        _hashes.Add(GetHash(x, y));

        if (_gemLogic.Roll())
        {
            CreateGem(tile.X, tile.Y);
        }
        _gemLogic.Step();
    }

    private void CreateGem(int x, int y)
    {
        var gem = PoolingObject<Gem>.GetNew();
        gem.X = x;
        gem.Y = y;
        gem.transform.localPosition = new Vector3(x, 0.2f, y);
        _gems.Add(gem);
    }

    private void FillField()
    {
        var corridorSize = GameConstants.CorridorSize;
        while (_tiles.Count < GameConstants.VisibleTiles * corridorSize)
        {
            var forward = Random.value > 0.5f;
            var w = 1;
            var h = 1;
            if (forward)
            {
                _lastTileY++;
                w = corridorSize;
            }
            else
            {
                _lastTileX++;
                h = corridorSize;
            }
            CreateBlock(_lastTileX, _lastTileY, w, h);
        }
    }

    private void RecycleTiles()
    {
        for (var i = 0; i < _tiles.Count; i++)
        {
            var tile = _tiles[i];
            if (tile.X < Mathf.RoundToInt(_ball.X) || tile.Y < Mathf.RoundToInt(_ball.Y))
            {
                tile.Fall();
                _tiles.Remove(tile);
                _hashes.Remove(GetHash(tile.X, tile.Y));
                i--;
            }
        }
    }

    private void CollectGems()
    {
        foreach (var gem in _gems)
        {
            if (gem.X == Mathf.RoundToInt(_ball.X) && gem.Y == Mathf.RoundToInt(_ball.Y))
            {
                gem.Collect();
                _gems.Remove(gem);
                _score++;
                return;
            }
        }
    }

    private void CheckFall()
    {
        var x = Mathf.RoundToInt(_ball.X);
        var y = Mathf.RoundToInt(_ball.Y);
        var hash = GetHash(x, y);
        if (!_hashes.Contains(hash))
        {
            _ball.Fall();
            _ball = null;
        }
        
        foreach (var gem in _gems)
        {
            hash = GetHash(gem.X, gem.Y);
            if (!_hashes.Contains(hash))
            {
                gem.Fall();
                _gems.Remove(gem);
                return;
            }
        }
    }

    private uint GetHash(int x, int y)
    {
        return (uint) ((x * 0x1f1f1f1f) ^ y);
    }

    private void Update()
    {
        var mouseDown = Input.GetMouseButtonDown((int) MouseButton.LeftMouse);
        
        if (_gameStarted)
        {
            if (!_ball)
            {
                _gameStarted = false;
                return;
            }
            
            if (mouseDown)
            {
                _ball.Switch();
            }

            CollectGems();
            RecycleTiles();
            FillField();
            CheckFall();
        }
        else if (mouseDown)
        {
            NewGame();
        }
    }
}
