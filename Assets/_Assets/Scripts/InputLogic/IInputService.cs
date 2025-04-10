using System;
using UnityEngine;

namespace _Assets.Scripts.InputLogic
{
    public interface IInputService : IDisposable
    {
        public event Action<Vector2> MoveInput;
        public event Action AttackInput;
        public event Action SwapSpellLeft;
        public event Action SwapSpellRight;
    }
}