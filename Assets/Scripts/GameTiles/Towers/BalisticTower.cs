using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class BalisticTower : Tower
    {
        [SerializeField, Range(0f, 10f)] private float _shootPerSecond = 1f;
        [SerializeField] private float _rocketBlastRaduis;
        [SerializeField, Range(0f, 200f)] private float _damage = 40f;
        [SerializeField] private Transform _mortar;
        [SerializeField] private Transform _rotator;
        private float _launchSpeed;
        private float _launchProgress;
        public override TowerType Type => TowerType.Balistic;


        public override void GameUpdate()
        {
            _launchProgress += Time.deltaTime * _shootPerSecond;
            while (_launchProgress >= 1f)
            {
                if (ObtainedTarget(out var target))
                {
                    Launch(target);
                    _launchProgress -= 1f;
                }
                else
                {
                    _launchProgress = 0.999f;
                }
            }
        }
        private void Awake()
        {
            OnValidate();
        }
        private void OnValidate()
        {
            float x = _targetingRange + 0.251f;
            float y = -_mortar.position.y;

            _launchSpeed = Mathf.Sqrt(9.81f * (y + Mathf.Sqrt(x * x + y * y)));
        }

        private void Launch(TargetPoint target)
        {
            var startPoint = _mortar.position;
            var targetPoint = target.Position;

            targetPoint.y = 0;

            Vector3 direction;
            direction.x = targetPoint.x - startPoint.x;
            direction.y = 0;
            direction.z = targetPoint.z - startPoint.z;

            float x = direction.magnitude;
            float y = -startPoint.y;
            direction /= x;

            float g = 9.81f;
            float s = _launchSpeed;
            float s2 = s * s;

            float r = s2 * s2 - g * (g * x * x + 2f * y * s2);
            r = Mathf.Max(0, r);

            float tanTheta = (s2 + Mathf.Sqrt(r)) / (g * x);
            float cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
            float sinTheta = cosTheta * tanTheta;

            _mortar.localRotation = Quaternion.LookRotation(direction);
            Game.SpawnShell().Initialize(startPoint, targetPoint,
                new Vector3(s * cosTheta * direction.x, s * sinTheta, s * cosTheta * direction.z), _rocketBlastRaduis, _damage);

        }
    }
}
