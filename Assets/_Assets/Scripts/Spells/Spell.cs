using System;
using System.Linq;
using System.Threading;
using _Assets.Scripts.CharacterBaseLogic;
using _Assets.Scripts.Configs;
using _Assets.Scripts.Spells.Enum;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Assets.Scripts.Spells
{
    public class Spell : MonoBehaviour, ISpell
    {
        public event Action<Spell> OnDeath;
        
        [SerializeField] private float destroyDuration;
        
        public SpellType SpellType { get; private set; }

        private SpellConfig _config;
        private bool _destroyed;
        private bool _casted;
        private CancellationTokenSource _cts;
        
        private void Update()
        {
            if (_destroyed || !_casted)
                return;
            
            Move();
        }

        public void Initialize(SpellConfig config)
        {
            _destroyed = false;
            _casted = false;
            _config = config;
            SpellType = config.SpellType;
            transform.localScale = config.Size;
        }

        public virtual void Cast()
        {
            _casted = true;
            _cts = new CancellationTokenSource();
            
            DestroyAfterDelay(_cts.Token).Forget();
        }

        public void Dispose()
        {
            _destroyed = true;
            _casted = false;
            OnDestroy();
        }

        private void OnDestroy()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }
        }

        protected virtual void Move()
        {
            transform.position += transform.up * (_config.Speed * Time.deltaTime);

            var foundCollider = Physics2D.OverlapCircle(transform.position, 0.1f);
            
            if (foundCollider != null)
                if (foundCollider.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(_config.Damage);
                    HandleDestroy(destroyDuration);
                }
        }

        private async UniTaskVoid DestroyAfterDelay(CancellationToken token)
        {
            await UniTask.Delay(_config.DestroyAfterInMillis, cancellationToken: token);
            HandleDestroy(0);
        }

        protected virtual void HandleDestroy(float duration)
        {
            if (_destroyed)
                return;

            _destroyed = true;
            
            transform.DOScale(0f, duration).SetEase(Ease.InBack).OnComplete(() => OnDeath?.Invoke(this));
        }
    }
}