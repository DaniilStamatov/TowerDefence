using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "Factories", menuName = "Factories/ContentFactory")]
    public class GameTileContentFactory : GeneralFactory
    {
        [SerializeField] private GameTileContent _obstaclePrefab;
        [SerializeField] private GameTileContent _destinationPrefab;
        [SerializeField] private GameTileContent _wallPrefab;
        [SerializeField] private GameTileContent _spawnPoint;
        [SerializeField] private Tower[] _towerPrefabs;


        public void Recycle(GameTileContent content)
        {
            Destroy(content.gameObject);
        }

        public GameTileContent Get(GameTileContentType type)
        {
            switch (type)
            {
                case GameTileContentType.Empty: return Get(_obstaclePrefab);
                case GameTileContentType.Destination: return Get(_destinationPrefab);
                case GameTileContentType.Wall: return Get(_wallPrefab);
                case GameTileContentType.SpawnPoint: return Get(_spawnPoint);
            }
            return null;
        }


        public Tower Get(TowerType type)
        {
            var prefab = _towerPrefabs[(int)type];
            return Get(prefab);
        }
        private T Get<T>(T prefab) where T : GameTileContent
        {
            var instance = MoveToScene(prefab);
            instance.OriginFactory = this;
            return instance;
        }

    }
}
