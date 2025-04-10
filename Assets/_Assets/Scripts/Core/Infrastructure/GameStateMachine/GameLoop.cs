using _Assets.Scripts.Core.Infrastructure.Configs;
using _Assets.Scripts.Core.Infrastructure.GameStateMachine.GameLoopStates;
using _Assets.Scripts.Core.Infrastructure.SceneManagement;
using _Assets.Scripts.Core.Infrastructure.WindowManagement;
using _Assets.Scripts.EnemyLogic.Spawner;
using _Assets.Scripts.HeroLogic.Factory;
using _Assets.Scripts.UI.Factory;
using UnityEngine;
using Zenject;

namespace _Assets.Scripts.Core.Infrastructure.GameStateMachine
{
    public class GameLoop : MonoBehaviour
    {
        private StateMachine.StateMachine _stateMachine;
        private ISceneLoader _sceneLoader;
        private WindowProvider _windowProvider;
        private GameConfig _gameConfig;
        private IHeroFactory _heroFactory;
        private IScreenFactory _screenFactory;
        private IEnemySpawner _enemySpawner;
        
        public ISceneLoader SceneLoader => _sceneLoader;
        public WindowProvider WindowProvider => _windowProvider;
        public GameConfig GameConfig => _gameConfig;
        public IHeroFactory HeroFactory => _heroFactory;
        public IScreenFactory ScreenFactory => _screenFactory;
        public IEnemySpawner EnemySpawner => _enemySpawner;

        public bool IsInitialized { get; private set; }
        
        [Inject]
        public void Construct(ISceneLoader sceneLoader, WindowProvider windowProvider,GameConfig gameConfig)
        {
            _sceneLoader = sceneLoader;
            _windowProvider = windowProvider;
            _gameConfig = gameConfig;
        }

        public void Initialize()
        {
            UpdateSceneContext();
            InitializeStateMachine();
            SceneLoader.SceneLoaded += UpdateSceneContext;
            
            _stateMachine.Enter<LoadLevelState, string>(_gameConfig.StartLevelScene);
        }

        public void ResetStateMachine() =>
            _stateMachine.Enter<LoadLevelState, string>(_gameConfig.StartLevelScene);

        private void InitializeStateMachine()
        {
            _stateMachine = new StateMachine.StateMachine();
            _stateMachine.AddState(new LoadLevelState(_stateMachine, this));
            _stateMachine.AddState(new StartGameState(_stateMachine, this));
            _stateMachine.AddState(new EndGameState(_stateMachine, this));
            
            IsInitialized = true;
        }

        private void UpdateSceneContext()
        {
            var sceneContext = FindFirstObjectByType<SceneContext>();
            if (sceneContext == null)
                return;
            
            _heroFactory = sceneContext.Container.TryResolve<IHeroFactory>();
            _enemySpawner = sceneContext.Container.TryResolve<IEnemySpawner>();
            _screenFactory = sceneContext.Container.TryResolve<IScreenFactory>();
        }
    }
}