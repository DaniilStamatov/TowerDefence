using UnityEngine;
namespace Assets.Scripts
{
    public class Explosion : WarEntity
    {
        [SerializeField, Range(0f, 2f)] private float _duration;
        [SerializeField] private AnimationCurve _scaleCurve;
        [SerializeField] private AnimationCurve _colorCurve;

        private static int _colorPropertyId = Shader.PropertyToID("Color");
        private static MaterialPropertyBlock _materialPropertyBlock;

        private float _scale;
        private MeshRenderer _meshRenderer;
        private float _age;


        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }
        public void Initialize(Vector3 position, float blastRadius, float damage = 0f)
        {
            if (damage > 0)
            {
                TargetPoint.FillBuffer(position, blastRadius);
                for (int i = 0; i < TargetPoint.BufferedCount; i++)
                {
                    TargetPoint.GetBuffered(i).Enemy.TakeDamage(damage);
                }
            }
            

            transform.localPosition = position;
            _scale = 2 * blastRadius;
        }

        public override bool GameUpdate()
        {
            _age += Time.deltaTime;
            if (_age >= _duration)
            {
                OriginFactory.Recycle(this);
                return false;
            }
            

            if (_materialPropertyBlock == null)
            {
                _materialPropertyBlock = new MaterialPropertyBlock();
            }
            float t = _age / _duration;
            Color c = Color.red;
            c.a = _colorCurve.Evaluate(t);
            _materialPropertyBlock.SetColor(_colorPropertyId, c);
            _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
            transform.localScale = Vector3.one * _scale * _scaleCurve.Evaluate(t);
            return true;
        }
    }
}
