using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Ads;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IAdsService _adsService;
        private readonly AllServices _services;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, IAdsService adsService/*, AllServices services*/)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _adsService = adsService;
        }

        public void Enter()
        {
            _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
            _adsService.Initialize();
        }

        public void Exit()
        {
        }

        private void EnterLoadLevel() =>
            _stateMachine.Enter<LoadProgressState>();    
    }
}