using System;
using _Assets.Scripts.CharacterBaseLogic;
using DG.Tweening;
using UnityEngine;

namespace _Assets.Scripts.EnemyLogic
{
    public class EnemyAnimator : MonoBehaviour, IDisposable
    {
        [SerializeField] private Vector3 defaultScale;
        [SerializeField] private float deathDuration = 0.3f;
        private HealthComponent _health;

        public void Initialize(HealthComponent health)
        {
            _health = health;
            transform.localScale = defaultScale;
            
            health.OnDeath += HandleDeath;
        }

        public void Dispose() =>
            OnDestroy();

        private void OnDestroy() =>
            _health.OnDeath -= HandleDeath;
        
        private void HandleDeath() =>
            transform.DOScale(0f, deathDuration).SetEase(Ease.InBack);
    }
}