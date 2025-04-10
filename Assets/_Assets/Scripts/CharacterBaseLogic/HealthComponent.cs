using System;
using UnityEngine;

namespace _Assets.Scripts.CharacterBaseLogic
{
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        public event Action<float, float> OnHealthChanged;
        public event Action OnDeath;

        public float CurrentHealth => _currentHealth;
        public float MaxHealth => _maxHealth;
        public float Defense => _defense;
        public bool IsAlive => _currentHealth > 0;
        
        private float _currentHealth;
        private float _maxHealth;
        private float _defense;

        public void Initialize(float maxHealth, float defense)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
            _defense = defense;
        }

        public void Kill() =>
            TakeDamage(_currentHealth, ignoreDefense: true);

        public void TakeDamage(float damage, bool ignoreDefense = false)
        {
            if (!IsAlive)
                return;
            
            var actualDamage = Mathf.Max(0f, damage * (ignoreDefense ? 1 : 1 - _defense));
            _currentHealth = Mathf.Max(0f, _currentHealth - actualDamage);
        
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        
            if (_currentHealth <= 0)
                OnDeath?.Invoke();
        }
    }
}