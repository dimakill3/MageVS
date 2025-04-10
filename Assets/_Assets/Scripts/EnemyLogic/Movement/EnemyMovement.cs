using System;
using _Assets.Scripts.CharacterBaseLogic;
using UnityEngine;

namespace _Assets.Scripts.EnemyLogic.Movement
{
    public class EnemyMovement : MonoBehaviour, IDisposable
    {
        public event Action<Transform> OnMovedToTarget;

        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private float _moveSpeed;
        private float _distanceToAttack;
        private float _damage;
        private Transform _target;
        private HealthComponent _health;

        private void Update()
        {
            if (_target == null || !_health.IsAlive)
                return;
            
            if (TargetNotReached())
                MoveToTarget();
            else
                OnMovedToTarget?.Invoke(_target);
        }

        public void Initialize(float moveSpeed, float distanceToAttack, HealthComponent health)
        {
            _moveSpeed = moveSpeed;
            _distanceToAttack = distanceToAttack;
            _health = health;
        }

        public void Dispose() =>
            _target = null;

        public void SetTargetTransform(Transform targetTransform) =>
            _target = targetTransform;

        private bool TargetNotReached()
        {
            var distance = Vector3.Distance(transform.position, _target?.position ?? transform.position);
            return distance > _distanceToAttack;
        }

        private void MoveToTarget()
        {
            RotateToTarget();
            
            transform.position = Vector3.MoveTowards(transform.position, _target?.position ?? transform.position,
                _moveSpeed * Time.deltaTime);
        }

        private void RotateToTarget()
        {
            Vector2 direction = (_target.position - transform.position).normalized;
            
            var dotProduct = Vector2.Dot(transform.right, direction);

            if (dotProduct < 0)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;
        }
    }
}