using System.Collections.Generic;
using System.IO;
using System.Linq;
using _Assets.Scripts.Configs;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _Assets.Scripts.Core.Infrastructure.Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [ValueDropdown("GetBuildScenes")]
        public string StartLevelScene;
        public HeroConfig HeroConfig;
        public SpawnConfig SpawnConfig;
        public List<EnemyConfig> EnemyConfigs;
        public List<SpellConfig> SpellConfigs;
        public InputConfig InputConfig;
        public Vector2Int MapSize;

#if UNITY_EDITOR
        private static string[] GetBuildScenes()
        {
            return EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => Path.GetFileNameWithoutExtension(scene.path))
                .ToArray();
        }
#endif
    }
}