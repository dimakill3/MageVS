using _Assets.Scripts.Core.Infrastructure.GameStateMachine;

namespace _Assets.Scripts.Core.Infrastructure.StateMachine
{
    public abstract class GameLoopParamState<TParam> : IParamState<TParam>
    {
        protected StateMachine StateMachine;
        protected GameLoop GameLoop;

        public GameLoopParamState(StateMachine stateMachine, GameLoop gameLoop)
        {
            StateMachine = stateMachine;
            GameLoop = gameLoop;
        }

        public abstract void OnEnter(TParam sceneName);

        public abstract void OnExit();
    }
}