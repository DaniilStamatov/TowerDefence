using UnityEngine;
namespace Assets.Scripts
{
    [CreateAssetMenu]
    public class WarFactory : GeneralFactory
    {
        [SerializeField] private Rocket _rocketPrefab;
        [SerializeField] private Explosion _explosionPrefab;

        public Rocket Rocket => Get(_rocketPrefab);

        public Explosion Explosion => Get(_explosionPrefab);

        private T Get<T>(T prefab) where T : WarEntity
        {
            T instance = MoveToScene(prefab);
            instance.OriginFactory = this;
            return instance;
        }

        public void Recycle(WarEntity entity)
        {
            Destroy(entity.gameObject);
        }
    }
}
