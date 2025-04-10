using System;
using _Assets.Scripts.CharacterBaseLogic;
using _Assets.Scripts.EnemyLogic.Movement;
using UnityEngine;

namespace _Assets.Scripts.EnemyLogic
{
    public class EnemyAttack : MonoBehaviour, IDisposable
    {
        private float _damage;
        private HealthComponent _health;
        private EnemyMovement _movement;

        public void Initialize(float damage, EnemyMovement movement, HealthComponent health)
        {
            _movement = movement;
            _health = health;
            _damage = damage;
            
            _movement.OnMovedToTarget += Attack;
        }

        public void Dispose() =>
            OnDestroy();

        private void OnDestroy() =>
            _movement.OnMovedToTarget -= Attack;

        private void Attack(Transform target)
        {
            if (target.TryGetComponent(out IDamageable damageable)) 
                damageable.TakeDamage(_damage);
            
            _health.Kill();
            _movement.OnMovedToTarget -= Attack;
        }
    }
}