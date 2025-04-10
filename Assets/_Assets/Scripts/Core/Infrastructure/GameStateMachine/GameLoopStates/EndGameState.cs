using _Assets.Scripts.Core.Infrastructure.StateMachine;

namespace _Assets.Scripts.Core.Infrastructure.GameStateMachine.GameLoopStates
{
    public class EndGameState : GameLoopState
    {
        public EndGameState(StateMachine.StateMachine stateMachine, GameLoop gameLoop) : base(stateMachine, gameLoop)
        {
        }

        public override void OnEnter() =>
            StateMachine.Enter<LoadLevelState, string>(GameLoop.GameConfig.StartLevelScene);

        private void OnLoaded() =>
            StateMachine.Enter<StartGameState>();

        public override void OnExit()
        {
            
        }
    }
}