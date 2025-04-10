using System;
using System.Threading.Tasks;
using UnityEngine;

namespace _Assets.Scripts.HeroLogic.Factory
{
    public interface IHeroFactory : IDisposable
    {
        public Hero CreatedHero { get; }
        public Task<Hero> CreateHero(Vector3 at = new());
    }
}