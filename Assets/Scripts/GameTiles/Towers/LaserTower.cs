using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class LaserTower : Tower
    {
        [SerializeField] private Transform _laserBeam;
        [SerializeField] private Transform _turret;
        [SerializeField, Range(1f, 100f)] private float _damagePerSecond = 10f;


        private Vector3 _laserBeamScale;
        private Vector3 _laserBeamPosition;
        private TargetPoint _target;

        public override TowerType Type => TowerType.Laser;

        private void Awake()
        {
            _laserBeamScale = _laserBeam.localScale;
            _laserBeamPosition = _laserBeam.localPosition;
        }
        public override void GameUpdate()
        {
            if (ObtainedTarget(out _target) || IsTargetTracked(ref _target))
            {
                Shoot();
            }
            else
            {
                _laserBeam.localScale = Vector3.zero;
            }
        }

        private void Shoot()
        {
            var point = _target.Position;
            _turret.LookAt(point);
            _laserBeam.localRotation = _turret.localRotation;

            var distance = Vector3.Distance(_turret.position, point);
            _laserBeamScale.z = distance;
            _laserBeam.localScale = _laserBeamScale;
            _laserBeam.localPosition = _laserBeamPosition + 0.5f * distance * _laserBeam.forward;
            _target.Enemy.TakeDamage(_damagePerSecond * Time.deltaTime);
        }
    }
}
