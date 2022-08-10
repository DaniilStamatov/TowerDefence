using UnityEngine;
namespace Assets.Scripts
{
    public class Rocket : WarEntity
    {
        private Vector3 _startPoint, _targetPoint, _launchVecocity;

        private float _damage, _blastRadius;

        private float _age;
        public void Initialize(Vector3 startPoint, Vector3 targetPoint, Vector3 launchVelocity, float blastRadius, float damage)
        {
            _startPoint = startPoint;
            _targetPoint = targetPoint;
            _launchVecocity = launchVelocity;
            _blastRadius = blastRadius;
            _damage = damage;
        }

        public override bool GameUpdate()
        {
            _age += Time.deltaTime;
            Vector3 point = _startPoint + _launchVecocity * _age;
            point.y -= 0.5f * 9.81f * _age * _age;

            if (point.y <= 0)
            {
                Game.SpawnExplosion().Initialize(_targetPoint, _blastRadius, _damage);
                OriginFactory.Recycle(this);
                return false;
            }
            transform.localPosition = point;

            Vector3 direction = _launchVecocity;
            direction.y -= 9.81f * _age;
            transform.localRotation = Quaternion.LookRotation(direction);

            Game.SpawnExplosion().Initialize(point, 0.1f);
            return true;
        }
    }
}
