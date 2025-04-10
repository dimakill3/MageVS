using _Assets.Scripts.Core.Infrastructure.GameStateMachine;
using UnityEngine;

namespace _Assets.Scripts.Core.Infrastructure.EntryPoint
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private GameLoop gameLoop;

        private void Awake() =>
            DontDestroyOnLoad(this);

        private void Start()
        {
            if (gameLoop.IsInitialized)
                gameLoop.ResetStateMachine();
            else
                gameLoop.Initialize();
        }
    }
}