using _Assets.Scripts.Core.Infrastructure.StateMachine;
using _Assets.Scripts.Core.UI;

namespace _Assets.Scripts.Core.Infrastructure.GameStateMachine.GameLoopStates
{
    public class LoadLevelState : GameLoopParamState<string>
    {
        private readonly LoadingScreen _loadingScreen;

        public LoadLevelState(StateMachine.StateMachine stateMachine, GameLoop gameLoop) : base(stateMachine, gameLoop) =>
            _loadingScreen = GameLoop.WindowProvider.GetWindow<LoadingScreen>();

        public override void OnEnter(string sceneName)
        {
            _loadingScreen.Show();
            GameLoop.SceneLoader.Load(sceneName, OnLoaded, true);
        }

        public override void OnExit()
        {
        }

        private void OnLoaded()
        {
            StateMachine.Enter<StartGameState>();
            _loadingScreen.Hide();
        }
    }
}