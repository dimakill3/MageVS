using UnityEngine;

namespace _Assets.Scripts.Core.Infrastructure.Configs
{
    [CreateAssetMenu(fileName = "InputConfig", menuName = "Configs/InputConfig")]
    public class InputConfig : ScriptableObject
    {
        public KeyCode AttackKey;
        public KeyCode SwapSpellLeftKey;
        public KeyCode SwapSpellRightKey;
    }
}