using System;
using _Assets.Scripts.CharacterBaseLogic;
using _Assets.Scripts.Configs;
using _Assets.Scripts.EnemyLogic.Enum;
using _Assets.Scripts.EnemyLogic.Movement;
using UnityEngine;

namespace _Assets.Scripts.EnemyLogic
{
    public class Enemy : MonoBehaviour, IDisposable
    {
        public event Action<Enemy> OnDeath;
        
        [SerializeField] private HealthComponent health;
        [SerializeField] private EnemyMovement movement;
        [SerializeField] private EnemyAttack attack;
        [SerializeField] private HealthView healthView;
        [SerializeField] private EnemyAnimator animator;

        public EnemyType EnemyType { get; private set; }

        public void Initialize(EnemyConfig config)
        {
            EnemyType = config.EnemyType;
            
            health.Initialize(config.MaxHealth, config.Defense);
            movement.Initialize(config.MoveSpeed, config.DistanceToAttack, health);
            attack.Initialize(config.Damage, movement, health);
            healthView.Initialize(health);
            animator.Initialize(health);
            
            health.OnDeath += HandleDeath;
        }

        public void SetTarget(Transform target) =>
            movement.SetTargetTransform(target);

        public void Dispose()
        {
            movement?.Dispose();
            attack?.Dispose();
            healthView?.Dispose();
            animator?.Dispose();
            
            OnDestroy();
        }

        private void OnDestroy() =>
            health.OnDeath -= HandleDeath;

        private void HandleDeath() =>
            OnDeath?.Invoke(this);
    }
}