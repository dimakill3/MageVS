using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "HeroConfig", menuName = "Configs/HeroConfig")]
    public class HeroConfig : ScriptableObject
    {
        public float MaxHealth = 100f;
        [Range(0f, 1f)]
        public float Defense = 0.5f;
        public float MoveSpeed = 5f;
        public AssetReference AddressableId;
    }
}