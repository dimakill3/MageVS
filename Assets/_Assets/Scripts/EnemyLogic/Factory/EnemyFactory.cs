using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Assets.Scripts.Configs;
using _Assets.Scripts.Core.Infrastructure.AssetManagement;
using _Assets.Scripts.Core.Infrastructure.Configs;
using _Assets.Scripts.EnemyLogic.Enum;
using _Assets.Scripts.HeroLogic.Factory;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;
using Object = UnityEngine.Object;

namespace _Assets.Scripts.EnemyLogic.Factory
{
    public class EnemyFactory : IEnemyFactory
    {
        private readonly DiContainer _diContainer;
        private readonly IAssetProvider _assetProvider;
        private readonly GameConfig _gameConfig;
        private readonly IHeroFactory _heroFactory;

        private readonly Dictionary<EnemyType, ObjectPool<Enemy>> _enemyPools = new ();
        private readonly Dictionary<EnemyType, GameObject> _enemyPrefabs = new ();
        
        [Inject]
        public EnemyFactory(DiContainer diContainer, IAssetProvider assetProvider, GameConfig gameConfig,
            IHeroFactory heroFactory)
        {
            _diContainer = diContainer;
            _assetProvider = assetProvider;
            _gameConfig = gameConfig;
            _heroFactory = heroFactory;
        }

        public async Task<Enemy> CreateEnemy(EnemyConfig enemyConfig, Vector3 at = new())
        {
            var key = enemyConfig.EnemyType;
            
            if (!_enemyPools.ContainsKey(key))
                await InitializePoolForEnemy(enemyConfig);

            var enemy = _enemyPools[key].Get();
            
            enemy.transform.position = at;
            enemy.transform.rotation = Quaternion.identity;
            enemy.Initialize(enemyConfig);
            enemy.SetTarget(_heroFactory.CreatedHero?.transform);
            
            return enemy;
        }

        public void Dispose() =>
            _enemyPools.Values.ForEach(pool => pool.Dispose());

        private async Task InitializePoolForEnemy(EnemyConfig enemyConfig)
        {
            var key = enemyConfig.EnemyType;

            GameObject prefab;
            
            if (!_enemyPrefabs.ContainsKey(key))
            {
                prefab = await _assetProvider.Load<GameObject>(enemyConfig.AddressableId);
                _enemyPrefabs[key] = prefab;
            }
            
            prefab = _enemyPrefabs[key];

            var pool = new ObjectPool<Enemy>(
                createFunc: () => CreateEnemyInstance(prefab),
                actionOnGet: OnEnemyGet,
                actionOnRelease: OnEnemyRelease,
                actionOnDestroy: OnEnemyDestroy,
                defaultCapacity: _gameConfig.SpawnConfig.MaxEnemiesOnScreen,
                maxSize: _gameConfig.SpawnConfig.MaxEnemiesOnScreen
            );
            
            _enemyPools[key] = pool;
        }

        private Enemy CreateEnemyInstance(GameObject prefab)
        {
            var enemy = _diContainer.InstantiatePrefabForComponent<Enemy>(prefab, Vector3.zero, Quaternion.identity, null);
            enemy.OnDeath += ReleaseEnemy;
            return enemy;
        }

        private void ReleaseEnemy(Enemy enemy) => 
            _enemyPools[enemy.EnemyType].Release(enemy);

        private void OnEnemyGet(Enemy enemy) =>
            enemy.gameObject.SetActive(true);

        private void OnEnemyRelease(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
            enemy.Dispose();
        }

        private void OnEnemyDestroy(Enemy enemy)
        {
            enemy.OnDeath -= ReleaseEnemy;
            Object.Destroy(enemy.gameObject);
        }
    }
}