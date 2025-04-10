using System.Threading.Tasks;
using _Assets.Scripts.CameraLogic;
using _Assets.Scripts.Core.Infrastructure.AssetManagement;
using _Assets.Scripts.Core.Infrastructure.Configs;
using _Assets.Scripts.InputLogic;
using UnityEngine;
using Zenject;

namespace _Assets.Scripts.HeroLogic.Factory
{
    public class HeroFactory : IHeroFactory
    {
        private readonly DiContainer _diContainer;
        private readonly IAssetProvider _assetProvider;
        private readonly GameConfig _gameConfig;
        private readonly IInputService _inputService;

        public Hero CreatedHero { get; private set; }

        [Inject]
        public HeroFactory(DiContainer diContainer, IAssetProvider assetProvider, GameConfig gameConfig,
            IInputService inputService)
        {
            _diContainer = diContainer;
            _assetProvider = assetProvider;
            _gameConfig = gameConfig;
            _inputService = inputService;
        }

        public async Task<Hero> CreateHero(Vector3 at = new())
        {
            var hero = _diContainer.InstantiatePrefabForComponent<Hero>(
                await _assetProvider.Load<GameObject>(_gameConfig.HeroConfig.AddressableId), at, Quaternion.identity, null);

            hero.Initialize(_gameConfig, _inputService);
            CameraFollow(hero.gameObject);
            CreatedHero = hero;
            return hero;
        }

        public void Dispose()
        {
            if (CreatedHero != null)
                CreatedHero.Dispose();
            
            CreatedHero = null;
        }

        private void CameraFollow(GameObject objectToFollow)
        {
            if (Camera.main != null)
                Camera.main
                    .GetComponent<CameraFollow>()
                    .Follow(objectToFollow);
        }
    }
}