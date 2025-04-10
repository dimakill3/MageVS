using System;

namespace _Assets.Scripts.EnemyLogic.Spawner
{
    public interface IEnemySpawner : IDisposable
    {
        void StartSpawning();
    }
}