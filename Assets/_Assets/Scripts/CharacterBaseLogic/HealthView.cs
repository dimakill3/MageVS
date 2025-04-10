using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Assets.Scripts.CharacterBaseLogic
{
    public class HealthView : MonoBehaviour, IDisposable
    {
        [SerializeField] private Canvas progressCanvas;
        [SerializeField] private Image progress;
        
        private HealthComponent _health;

        public void Initialize(HealthComponent health)
        {
            _health = health;
            progressCanvas.gameObject.SetActive(true);
            HandleHealthChanged(_health.CurrentHealth, _health.MaxHealth);
            
            _health.OnHealthChanged += HandleHealthChanged;
            _health.OnDeath += HandleDeath;
        }

        public void Dispose() =>
            OnDestroy();

        private void OnDestroy()
        {
            _health.OnHealthChanged -= HandleHealthChanged;
            _health.OnDeath -= HandleDeath;
        }

        private void HandleDeath() =>
            progressCanvas.gameObject.SetActive(false);

        private void HandleHealthChanged(float currentHealth, float maxHealth) =>
            progress.fillAmount = currentHealth / maxHealth;
    }
}