using UnityEngine;

namespace Assets.Scripts
{
    public class TargetPoint : MonoBehaviour
    {
        public Enemy Enemy { get; private set; }

        public Vector3 Position => transform.position;

        public float ColliderSize { get; private set; }

        private static Collider[] _buffer = new Collider[100];
        private const int ENEMY_LAYER_MASK = 1 << 9;

        public static int BufferedCount { get; private set; }
        public bool IsEnabled { get; set; }
 
        private void Awake()
        {
            Enemy = transform.root.GetComponent<Enemy>();
            ColliderSize = GetComponent<SphereCollider>().radius * transform.localScale.x;
        }

        public static bool FillBuffer(Vector3 position, float range)
        {
            var top = position;
            top.y += 3f;

            BufferedCount = Physics.OverlapCapsuleNonAlloc(position, top, range, _buffer, ENEMY_LAYER_MASK);
            return BufferedCount > 0;
        }


        public static TargetPoint GetBuffered(int index)
        {
            var target = _buffer[index].GetComponent<TargetPoint>();
            return target;
        }

    }
}
