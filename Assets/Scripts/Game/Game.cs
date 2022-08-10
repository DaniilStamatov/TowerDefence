using Assets.Scripts;
using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Vector2Int _boardSize;
    [SerializeField] private GameBoard _board;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameTileContentFactory _contentFactory;
    [SerializeField] private WarFactory _warFactory;
    [SerializeField] private GameScenario _scenario;
    [SerializeField, Range(10, 100)] private int _startPlayerHealth = 10;
    [SerializeField, Range(5f, 30f)] private float _prepareTime = 5f;

    private bool _scenarioIsInProgress;
    private int _currentPlayerHealth;


    private GameScenario.State _activeScenario;

    private GameBehaviourCollection _enemies = new GameBehaviourCollection();
    private GameBehaviourCollection _nonEnemies = new GameBehaviourCollection();

    private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);
    private TowerType _currentTowerType;
    private static Game _instance;
    private bool _isPaused;

    private void OnEnable()
    {
        _instance = this;
    }

    private void Start()
    {
        _board.Initialize(_boardSize, _contentFactory);
        BeginNewGame();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isPaused = !_isPaused;
            Time.timeScale = _isPaused ? 0f : 1f;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            BeginNewGame();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentTowerType = TowerType.Laser;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentTowerType = TowerType.Balistic;
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleWallTouch();
        }
        if (Input.GetMouseButtonDown(1))
        {
            HandleTouch();
        }
        if (_scenarioIsInProgress)
        {
            if (_currentPlayerHealth <= 0)
            {
                Debug.Log("Defeat");
                BeginNewGame();
            }
            if (!_activeScenario.Progress() && _enemies.IsEmpty)
            {
                Debug.Log("Victory");
                BeginNewGame();
                _activeScenario.Progress();
            }
        }

        _enemies.GameUpdate();
        Physics.SyncTransforms();
        _board.GameUpdate();
        _nonEnemies.GameUpdate();
    }


    public static void SpawnEnemy(EnemyFactory enemyFactory, EnemyType type)
    {
        GameTile tile = _instance._board.GetSpawnPoint(Random.Range(0, _instance._board.SpawnPointCount));
        Enemy enemy = enemyFactory.Get(type);
        enemy.SpawnOn(tile);
        _instance._enemies.Add(enemy);
    }

    private void HandleTouch()
    {
        GameTile tile = _board.GetTile(TouchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _board.ToggleDestination(tile);
            }
            _board.ToggleSpawnPoint(tile);
        }
    }
    private void HandleWallTouch()
    {
        GameTile tile = _board.GetTile(TouchRay);
        if (tile != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _board.ToggleTower(tile, _currentTowerType);
            }
            _board.ToggleWall(tile);
        }
    }

    public static Rocket SpawnShell()
    {
        var rocket = _instance._warFactory.Rocket;
        _instance._nonEnemies.Add(rocket);
        return rocket;
    }

    public static Explosion SpawnExplosion()
    {
        var explosion = _instance._warFactory.Explosion;
        _instance._nonEnemies.Add(explosion);
        return explosion;
    }

    private void BeginNewGame()
    {
        _scenarioIsInProgress = false;
        if (_prepareRoutine != null)
        {
            StopCoroutine(_prepareRoutine);
        }
        _enemies.Clear();
        _nonEnemies.Clear();
        _board.Clear();
        _currentPlayerHealth = _startPlayerHealth;
        _prepareRoutine = StartCoroutine(PrepareRoutine());
    }

    public static void EnemyReachedDestination()
    {
        _instance._currentPlayerHealth--;
    }

    private Coroutine _prepareRoutine;

    private IEnumerator PrepareRoutine()
    {
        yield return new WaitForSeconds(_prepareTime);
        _activeScenario = _scenario.Begin();
        _scenarioIsInProgress = true;
    }
}
