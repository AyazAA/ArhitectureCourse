using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        public LoadingCurtain Curtain;
        private Game _game;
        private IGameStateMachine _gameStateMachine;

        [Inject]
        public void Constructor(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            Initialize();
        }

        private void Initialize()
        { 
            _game = new Game(_gameStateMachine);
            _game.InitializeStateMachine(this, Curtain);
            _game.StateMachine.Enter<BootstrapState>();

            DontDestroyOnLoad(this);
        }
    }
}