using System;
using System.Threading;
using System.Threading.Tasks;
using _Assets.Scripts.Configs;
using UnityEngine;

namespace _Assets.Scripts.Spells.Factory
{
    public interface ISpellFactory : IDisposable
    {
        Task<Spell> CreateSpell(SpellConfig spellConfig, Vector3 at, Quaternion dir, CancellationToken token = default);
    }
}