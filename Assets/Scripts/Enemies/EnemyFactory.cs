using System;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu]
    public class EnemyFactory : GeneralFactory
    {
        [Serializable]
        class EnemyConfig
        {
            public Enemy Prefab;
            [FloatRangeSlider(0.5f, 2f)] public FloatRange Scale = new FloatRange(1f);
            [FloatRangeSlider(-0.4f, 0.4f)] public FloatRange PathOffset = new FloatRange(0f);
            [FloatRangeSlider(0.2f, 5f)] public FloatRange Speed = new FloatRange(1f);
            [FloatRangeSlider(10f, 1000f)] public FloatRange Health = new FloatRange(100f);
        }

        [SerializeField] private EnemyConfig _goblin, _medium, _golem;


        private EnemyConfig GetConfig(EnemyType type)
        {
            switch (type)
            {
                case EnemyType.Goblin: return _goblin;
                    case EnemyType.Medium: return _medium;
                    case EnemyType.Large: return _golem;

            }
            Debug.Log($"No config of this {type}");
            return _medium;

        }
        public Enemy Get(EnemyType type)
        {
            var config = GetConfig(type);
            Enemy instance = MoveToScene(config.Prefab);
            instance.OriginFactory = this;
            instance.Initialize(config.Scale.RandomValueInRange,
                config.PathOffset.RandomValueInRange,
                config.Speed.RandomValueInRange,
                config.Health.RandomValueInRange);
            return instance;
        }

        public void Recycle(Enemy enemy)
        {
            Destroy(enemy.gameObject);
        }
    }
}
