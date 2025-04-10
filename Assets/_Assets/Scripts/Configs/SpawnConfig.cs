using UnityEngine;

namespace _Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "SpawnConfig", menuName = "Configs/SpawnConfig")]
    public class SpawnConfig : ScriptableObject
    {
        public int MaxEnemiesOnScreen = 10;
        public float SpawnRadius = 20f;
    }
}