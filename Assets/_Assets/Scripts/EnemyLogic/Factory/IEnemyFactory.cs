using System;
using System.Threading.Tasks;
using _Assets.Scripts.Configs;
using UnityEngine;

namespace _Assets.Scripts.EnemyLogic.Factory
{
    public interface IEnemyFactory : IDisposable
    {
        Task<Enemy> CreateEnemy(EnemyConfig enemyConfig, Vector3 at = new());
    }
}