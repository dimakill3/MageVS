using System;

namespace _Assets.Scripts.CharacterBaseLogic
{
    public interface IDamageable
    {
        event Action<float, float> OnHealthChanged;
        event Action OnDeath;
        
        float CurrentHealth { get; }
        float MaxHealth { get; }
        float Defense { get; }
        bool IsAlive { get; }
    
        void TakeDamage(float damage, bool ignoreDefense = false);
    }
}