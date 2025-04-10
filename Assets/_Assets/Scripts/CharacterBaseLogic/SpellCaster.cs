using System;
using System.Collections.Generic;
using System.Threading;
using _Assets.Scripts.Configs;
using _Assets.Scripts.Core.Infrastructure.EventManagement;
using _Assets.Scripts.InputLogic;
using _Assets.Scripts.Spells;
using _Assets.Scripts.Spells.Enum;
using _Assets.Scripts.Spells.Factory;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Assets.Scripts.CharacterBaseLogic
{
    public class SpellCaster : MonoBehaviour, IDisposable
    {
        public event Action<int> OnSpellCasted;
        public event Action<SpellType> OnSpellChanged;
        
        [SerializeField] private Transform _spellSpawnPoint;

        public SpellType CurrentSpell => _spells.Count > 0 ? _spells[CurrentSpellIndex].SpellType : 0;

        private int _currentSpellIndex;
        private List<SpellConfig> _spells = new();
        private CancellationTokenSource _cancellationTokenSource;
        private ISpellFactory _spellFactory;
        private IInputService _inputService;

        private int CurrentSpellIndex
        {
            get => _currentSpellIndex;
            set
            {
                _currentSpellIndex = value;
                OnSpellChanged?.Invoke(_spells[_currentSpellIndex].SpellType);
            }
        }

        [Inject]
        public void Construct(ISpellFactory spellFactory, IInputService inputService)
        {
            _spellFactory = spellFactory;
            _inputService = inputService;

            _inputService.AttackInput += CastCurrentSpell;
            _inputService.SwapSpellLeft += SelectPreviousSpell;
            _inputService.SwapSpellRight += SelectNextSpell;
        }

        public void Dispose() =>
            OnDestroy();

        private void OnDestroy()
        {
            _inputService.AttackInput -= CastCurrentSpell;
            _inputService.SwapSpellLeft -= SelectPreviousSpell;
            _inputService.SwapSpellRight -= SelectNextSpell;

            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
            }
            
            _spells.Clear();
        }

        public void Initialize(List<SpellConfig> spells)
        {
            _spells.AddRange(spells);
            
            if (_spells.Count > 0)
                CurrentSpellIndex = 0;
        }

        private async void CastCurrentSpell()
        {
            if (_cancellationTokenSource != null || _spells.Count == 0)
                return;

            _cancellationTokenSource = new CancellationTokenSource();
            
            await CastCurrentSpell(_cancellationTokenSource.Token);
        }

        private async UniTask CastCurrentSpell(CancellationToken token)
        {
            var currentSpell = _spells[_currentSpellIndex];
            ISpell spell = await _spellFactory.CreateSpell(currentSpell, _spellSpawnPoint.position, transform.rotation, token);
            spell.Cast();
            
            OnSpellCasted?.Invoke(currentSpell.CooldownInMillis);
            await UniTask.Delay(currentSpell.CooldownInMillis, cancellationToken: token);
            
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }

        private void SelectNextSpell()
        {
            if (_spells.Count == 0)
                return;
            
            CurrentSpellIndex = (CurrentSpellIndex + 1) % _spells.Count;
        }

        private void SelectPreviousSpell()
        {
            if (_spells.Count == 0)
                return;
        
            CurrentSpellIndex = (CurrentSpellIndex - 1 + _spells.Count) % _spells.Count;
        }
    }
}