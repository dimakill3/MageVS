using _Assets.Scripts.Core.Infrastructure.StateMachine;
using _Assets.Scripts.EnemyLogic.Spawner;
using _Assets.Scripts.HeroLogic.Factory;
using _Assets.Scripts.UI.Factory;

namespace _Assets.Scripts.Core.Infrastructure.GameStateMachine.GameLoopStates
{
    public class StartGameState : GameLoopState
    {
        public StartGameState(StateMachine.StateMachine stateMachine, GameLoop gameLoop) : base(stateMachine, gameLoop)
        {
        }

        public override async void OnEnter()
        {
            await GameLoop.ScreenFactory.CreateHud();
            var hero = await GameLoop.HeroFactory.CreateHero();

            GameLoop.EnemySpawner.StartSpawning();
            hero.OnDeath += OnGameEnd;
        }

        public override void OnExit()
        {
            GameLoop.EnemySpawner.Dispose();
            GameLoop.HeroFactory.Dispose();
            GameLoop.ScreenFactory.Dispose();
        }

        private void OnGameEnd() =>
            StateMachine.Enter<EndGameState>();
    }
}