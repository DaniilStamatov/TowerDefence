using UnityEngine;

namespace Assets.Scripts
{
    [SelectionBase]
    public class GameTileContent : MonoBehaviour
    {
        [SerializeField] private GameTileContentType _type;
        public GameTileContentType Type => _type;

        public GameTileContentFactory OriginFactory { get; set; }

        public bool IsBlockingPath => Type == GameTileContentType.Wall || Type == GameTileContentType.Tower;

        public void Recycle()
        {
            OriginFactory.Recycle(this);
        }

        public virtual void GameUpdate()
        {

        }
    }

    public enum GameTileContentType
    {
        Empty,
        Destination,
        Wall,
        SpawnPoint,
        Tower
    }

    public enum TowerType
    {
        Laser,
        Balistic
    }
}
