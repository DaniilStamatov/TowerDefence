using System;
using UnityEngine;
namespace Assets.Scripts
{
    public abstract class Tower : GameTileContent
    {
        [SerializeField, Range(1.5f, 10f)] protected float _targetingRange = 1.5f;
        public abstract TowerType Type { get; }


        protected bool ObtainedTarget(out TargetPoint target)
        {

            if (TargetPoint.FillBuffer(transform.localPosition, _targetingRange))
            {
                target = TargetPoint.GetBuffered(0);
                return true;
            }
            target = null;
            return false;
        }

        protected bool IsTargetTracked(ref TargetPoint target)
        {
            if (target == null) return false;

            var targetPosition = target.Position;
            if (Vector3.Distance(transform.position, targetPosition) > _targetingRange + target.ColliderSize * target.Enemy.Scale || target.IsEnabled == false)
            {
                target = null;
                return false;
            }
            return true;
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            var position = transform.localPosition;
            position.y += 0.01f;

            Gizmos.DrawWireSphere(position, _targetingRange);
        }

    }
}
