using _Assets.Scripts.EnemyLogic.Factory;
using _Assets.Scripts.EnemyLogic.Spawner;
using _Assets.Scripts.HeroLogic.Factory;
using _Assets.Scripts.Spells.Factory;
using _Assets.Scripts.UI.Factory;
using Zenject;

namespace _Assets.Scripts.Installers
{
    public class FactoriesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IHeroFactory>()
                .To<HeroFactory>()
                .AsSingle();
         
            Container
                .Bind<IEnemyFactory>()
                .To<EnemyFactory>()
                .AsSingle();
            
            Container
                .Bind<IEnemySpawner>()
                .To<EnemySpawner>()
                .AsSingle();
            
            Container
                .Bind<ISpellFactory>()
                .To<SpellFactory>()
                .AsSingle();
            
            Container
                .Bind<IScreenFactory>()
                .To<ScreenFactory>()
                .AsSingle();
        }
    }
}