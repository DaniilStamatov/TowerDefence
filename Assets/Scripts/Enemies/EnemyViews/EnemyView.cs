using UnityEngine;

namespace Assets.Scripts.Enemies.EnemyViews
{
    public abstract class EnemyView : MonoBehaviour
    {
        public bool IsInited { get; private set; }
        protected Animator _animator;
        protected Enemy _enemy;

        protected static int DiedKey = Animator.StringToHash("Died");

        public virtual void Init(Enemy enemy)
        {
            _animator = GetComponent<Animator>();
            _enemy = enemy;
        }

        public virtual void Die()
        {
            _animator.SetBool(DiedKey, true);
        }

        public void OnSpawnAnimationComplete()
        {
            IsInited = true;
            GetComponent<TargetPoint>().IsEnabled = true;
        }
    }

}
