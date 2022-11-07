using Assets.CodeBase.Infrasstructure.AssetManagement;
using Assets.CodeBase.Infrasstructure.Factory;
using Assets.CodeBase.Infrasstructure.Services;
using Assets.CodeBase.Services.Input;
using UnityEngine;

namespace Assets.CodeBase.Infrasstructure.States
{
    public class BootstrapState : IState
    {
        private const string Initial = "Initial";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            
            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
        }

        public void Exit()
        {
        }

        private void EnterLoadLevel() =>
            _stateMachine.Enter<LoadLevelState, string>("Main");

        private void RegisterServices() 
        {
            _services.RegisterSingle<IInputService>(InputService());
            _services.RegisterSingle<IAsset>(new AssetProvider());
            _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAsset>()));
        }

        private static IInputService InputService()
        {
            if (Application.isEditor)
                return new StandoleInputService();
            else
                return new MobileInputService();
        }
    }
}