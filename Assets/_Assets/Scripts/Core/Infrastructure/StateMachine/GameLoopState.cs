using _Assets.Scripts.Core.Infrastructure.GameStateMachine;

namespace _Assets.Scripts.Core.Infrastructure.StateMachine
{
    public abstract class GameLoopState : IState
    {
        protected StateMachine StateMachine;
        protected GameLoop GameLoop;

        public GameLoopState(StateMachine stateMachine, GameLoop gameLoop)
        {
            StateMachine = stateMachine;
            GameLoop = gameLoop;
        }

        public virtual void OnExit()
        {
        }

        public virtual void OnEnter()
        {
        }
    }
}