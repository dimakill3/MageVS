using System;
using System.Threading;
using _Assets.Scripts.Configs;
using _Assets.Scripts.Core.Infrastructure.Configs;
using _Assets.Scripts.EnemyLogic.Factory;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace _Assets.Scripts.EnemyLogic.Spawner
{
    public class EnemySpawner : IEnemySpawner
    {
        private GameConfig _gameConfig;
        private SpawnConfig _spawnConfig => _gameConfig.SpawnConfig;
        private IEnemyFactory _enemyFactory;

        private int _currentMonstersCount;
        private CancellationTokenSource _cancellationTokenSource;

        [Inject]
        private void Construct(GameConfig gameConfig, IEnemyFactory enemyFactory)
        {
            _enemyFactory = enemyFactory;
            _gameConfig = gameConfig;
        }

        public void Dispose()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }

            _enemyFactory.Dispose();
        }

        public void StartSpawning()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            
            Spawning(_cancellationTokenSource.Token).Forget();
        }

        private async UniTaskVoid Spawning(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (_currentMonstersCount < _spawnConfig.MaxEnemiesOnScreen)
                {
                    SpawnMonster();
                    _currentMonstersCount++;
                }
                
                await UniTask.WaitForFixedUpdate();
            }
        }

        private async void SpawnMonster()
        {
            var selectedConfig = GetRandomMonsterConfig();
            var spawnPosition = GetRandomSpawnPosition();

            var enemy = await _enemyFactory.CreateEnemy(selectedConfig, spawnPosition);
            enemy.OnDeath += HandleEnemyDeath;
        }

        private void HandleEnemyDeath(Enemy enemy)
        {
            enemy.OnDeath -= HandleEnemyDeath;
            _currentMonstersCount--;
        }

        private EnemyConfig GetRandomMonsterConfig() =>
            _gameConfig.EnemyConfigs[Random.Range(0, _gameConfig.EnemyConfigs.Count)];

        private Vector3 GetRandomSpawnPosition()
        {
            var angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            
            var x = Mathf.Sin(angle) * _spawnConfig.SpawnRadius;
            var y = Mathf.Cos(angle) * _spawnConfig.SpawnRadius;
            
            return new Vector3(x, y, 0);
        }
    }
}