using _Assets.Scripts.Spells.Enum;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "SpellConfig", menuName = "Configs/SpellConfig")]
    public class SpellConfig : ScriptableObject
    {
        public SpellType SpellType;
        public Sprite Sprite;
        public float Damage;
        public float Speed;
        public int CooldownInMillis;
        public int DestroyAfterInMillis;
        public Vector3 Size;
        public AssetReference AddressableId;
    }
}