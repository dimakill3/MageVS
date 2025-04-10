using System;
using _Assets.Scripts.CharacterBaseLogic;
using _Assets.Scripts.Core.Infrastructure.Configs;
using _Assets.Scripts.HeroLogic.Movement;
using _Assets.Scripts.InputLogic;
using _Assets.Scripts.Spells.Enum;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Assets.Scripts.HeroLogic
{
    public class Hero : MonoBehaviour, IDisposable
    {
        public event Action OnDeath;
        
        [SerializeField] private HealthComponent health;
        [SerializeField] private HeroMovement movement;
        [SerializeField] private SpellCaster spellCaster;
        [SerializeField] private HealthView healthView;
        [SerializeField] private HeroAnimator animator;
        [SerializeField] private HeroNotifier heroNotifier;
        
        public void Initialize(GameConfig gameConfig, IInputService inputService)
        {
            var heroConfig = gameConfig.HeroConfig;
            var spellConfigs = gameConfig.SpellConfigs;
            
            health.Initialize(heroConfig.MaxHealth, heroConfig.Defense);
            movement.Initialize(heroConfig.MoveSpeed, inputService, gameConfig.MapSize);
            heroNotifier.Initialize(spellCaster);
            spellCaster.Initialize(spellConfigs);
            healthView.Initialize(health);
            animator.Initialize(health);

            health.OnDeath += HandleDeath;
        }
        
        public void Dispose()
        {
            movement?.Dispose();
            spellCaster?.Dispose();
            animator?.Dispose();
            healthView?.Dispose();
            heroNotifier?.Dispose();
            OnDestroy();
        }

        private void OnDestroy() =>
            health.OnDeath -= HandleDeath;

        private void HandleDeath() =>
            OnDeath?.Invoke();
    }
}