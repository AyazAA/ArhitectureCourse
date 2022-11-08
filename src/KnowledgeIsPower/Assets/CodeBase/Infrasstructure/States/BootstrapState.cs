using CodeBase.Infrasstructure.AssetManagement;
using CodeBase.Infrasstructure.Factory;
using CodeBase.Infrasstructure.Services;
using CodeBase.Infrasstructure.Services.Input;
using CodeBase.Infrasstructure.Services.PersistentProgress;
using CodeBase.Infrasstructure.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Infrasstructure.States
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
            _stateMachine.Enter<LoadProgressState>();    

        private void RegisterServices() 
        {
            _services.RegisterSingle<IInputService>(InputService());
            _services.RegisterSingle<IAsset>(new AssetProvider());
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAsset>()));
            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistentProgressService>(), _services.Single<IGameFactory>()));
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