using System;
using _Assets.Scripts.CharacterBaseLogic;
using _Assets.Scripts.Core.Infrastructure.EventManagement;
using _Assets.Scripts.Events;
using _Assets.Scripts.Spells.Enum;
using UnityEngine;
using Zenject;

namespace _Assets.Scripts.HeroLogic
{
    public class HeroNotifier : MonoBehaviour, IDisposable
    {
        private IEventProvider _eventProvider;
        private SpellCaster _spellCaster;

        [Inject]
        private void Construct(IEventProvider eventProvider) =>
            _eventProvider = eventProvider;

        public void Initialize(SpellCaster spellCaster)
        {
            _spellCaster = spellCaster;

            _spellCaster.OnSpellCasted += NotifySpellCasted;
            _spellCaster.OnSpellChanged += NotifySpellChanged;
        }

        public void Dispose()
        {
            _spellCaster.OnSpellCasted -= NotifySpellCasted;
            _spellCaster.OnSpellChanged -= NotifySpellChanged;
        }

        private void NotifySpellCasted(int cooldown) =>
            _eventProvider.Invoke(new SpellCasted(cooldown));

        private void NotifySpellChanged(SpellType spellType) =>
            _eventProvider.Invoke(new CurrentSpellChanged(spellType));
    }
}