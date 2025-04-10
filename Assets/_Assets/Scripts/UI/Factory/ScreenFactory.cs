using System.Threading.Tasks;
using _Assets.Scripts.Core.Infrastructure.AssetManagement;
using _Assets.Scripts.Core.Infrastructure.Configs;
using _Assets.Scripts.Core.Infrastructure.EventManagement;
using _Assets.Scripts.UI.HUD;
using UnityEngine;
using Zenject;

namespace _Assets.Scripts.UI.Factory
{
    public class ScreenFactory : IScreenFactory
    {
        private readonly DiContainer _diContainer;
        private readonly IAssetProvider _assetProvider;
        private readonly GameConfig _gameConfig;
        private readonly IEventProvider _eventProvider;

        public Hud CreatedHUD { get; private set; }
        
        [Inject]
        public ScreenFactory(DiContainer diContainer, IAssetProvider assetProvider, GameConfig gameConfig, IEventProvider eventProvider)
        {
            _diContainer = diContainer;
            _assetProvider = assetProvider;
            _gameConfig = gameConfig;
            _eventProvider = eventProvider;
        }

        public async Task<Hud> CreateHud()
        {
            var hud = _diContainer.InstantiatePrefabForComponent<Hud>(await _assetProvider.Load<GameObject>(AssetPath.HudPath));
            
            hud.Initialize(_eventProvider, _gameConfig.SpellConfigs.ToArray(), _gameConfig.InputConfig.SwapSpellLeftKey,
                _gameConfig.InputConfig.SwapSpellRightKey, _gameConfig.InputConfig.AttackKey);

            CreatedHUD = hud;
            return hud;
        }

        public void Dispose()
        {
            if (CreatedHUD != null)
            {
                CreatedHUD.Dispose();
                Object.Destroy(CreatedHUD.gameObject);
            }
            
            CreatedHUD = null;
        }
    }
}