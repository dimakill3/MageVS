using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using _Assets.Scripts.Configs;
using _Assets.Scripts.Core.Infrastructure.AssetManagement;
using _Assets.Scripts.Spells.Enum;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace _Assets.Scripts.Spells.Factory
{
    public class SpellFactory : ISpellFactory
    {
        private readonly DiContainer _diContainer;
        private readonly IAssetProvider _assetProvider;

        private readonly Dictionary<SpellType, ObjectPool<Spell>> _enemyPools = new ();
        private readonly Dictionary<SpellType, GameObject> _enemyPrefabs = new ();
        
        [Inject]
        public SpellFactory(DiContainer diContainer, IAssetProvider assetProvider)
        {
            _diContainer = diContainer;
            _assetProvider = assetProvider;
        }

        public async Task<Spell> CreateSpell(SpellConfig spellConfig, Vector3 at, Quaternion direction, CancellationToken token = default)
        {
            var key = spellConfig.SpellType;
            
            if (!_enemyPools.ContainsKey(key))
                await InitializePoolForSpell(spellConfig);

            var spell = _enemyPools[key].Get();
            
            spell.transform.position = at;
            spell.transform.rotation = direction;
            spell.Initialize(spellConfig);
            
            return spell;
        }

        public void Dispose() =>
            _enemyPools.Values.ForEach(pool => pool.Dispose());

        private async Task InitializePoolForSpell(SpellConfig spellConfig)
        {
            var key = spellConfig.SpellType;

            GameObject prefab;
            
            if (!_enemyPrefabs.ContainsKey(key))
            {
                prefab = await _assetProvider.Load<GameObject>(spellConfig.AddressableId);
                _enemyPrefabs[key] = prefab;
            }
            
            prefab = _enemyPrefabs[key];

            var pool = new ObjectPool<Spell>(
                createFunc: () => CreateSpellInstance(prefab),
                actionOnGet: OnSpellGet,
                actionOnRelease: OnSpellRelease,
                actionOnDestroy: OnSpellDestroy
            );
            
            _enemyPools[key] = pool;
        }

        private Spell CreateSpellInstance(GameObject prefab)
        {
            var spell = _diContainer.InstantiatePrefabForComponent<Spell>(prefab, Vector3.zero, Quaternion.identity, null);
            spell.OnDeath += ReleaseSpell;
            return spell;
        }

        private void ReleaseSpell(Spell spell) => 
            _enemyPools[spell.SpellType].Release(spell);

        private void OnSpellGet(Spell spell) =>
            spell.gameObject.SetActive(true);

        private void OnSpellRelease(Spell spell)
        {
            spell.gameObject.SetActive(false);
            spell.Dispose();
        }

        private void OnSpellDestroy(Spell spell)
        {
            spell.OnDeath -= ReleaseSpell;
            Object.Destroy(spell.gameObject);
        }
    }
}