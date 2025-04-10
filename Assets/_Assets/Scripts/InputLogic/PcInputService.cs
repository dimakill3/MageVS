using System;
using _Assets.Scripts.Core.Infrastructure.Configs;
using _Assets.Scripts.Core.Infrastructure.Mono;
using UnityEngine;

namespace _Assets.Scripts.InputLogic
{
    public class PcInputService : IInputService
    {
        private const string Horizontal = "Horizontal";
        private const string Vertical = "Vertical";
        
        private readonly MonoService _monoService;
        private readonly InputConfig _inputConfig;

        public event Action<Vector2> MoveInput;
        public event Action AttackInput;
        public event Action SwapSpellLeft;
        public event Action SwapSpellRight;
        
        
        public PcInputService(MonoService monoService, GameConfig gameConfig)
        {
            _monoService = monoService;
            _inputConfig = gameConfig.InputConfig;
            _monoService.OnTick += Update;
        }

        public void Dispose() =>
            _monoService.OnTick -= Update;

        private void Update()
        {
            UpdateMovement();
            UpdateKeys();
        }

        private void UpdateMovement() =>
            MoveInput?.Invoke(new Vector2(Input.GetAxisRaw(Horizontal), Input.GetAxisRaw(Vertical)));

        private void UpdateKeys()
        {
            if (Input.GetKeyDown(_inputConfig.AttackKey))
                AttackInput?.Invoke();
            
            if (Input.GetKeyDown(_inputConfig.SwapSpellLeftKey))
                SwapSpellLeft?.Invoke();
            
            if (Input.GetKeyDown(_inputConfig.SwapSpellRightKey))
                SwapSpellRight?.Invoke();
        }
    }
}