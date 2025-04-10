using _Assets.Scripts.EnemyLogic.Enum;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        public EnemyType EnemyType;
        public float MaxHealth;
        [Range(0f, 1f)]
        public float Defense;
        public float Damage;
        public float MoveSpeed;
        public float DistanceToAttack;
        public AssetReference AddressableId;
    }
}