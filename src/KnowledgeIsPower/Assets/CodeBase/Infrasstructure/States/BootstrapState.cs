using CodeBase.Infrasstructure.AssetManagement;
using CodeBase.Infrasstructure.Factory;
using CodeBase.Infrasstructure.Services;
using CodeBase.Infrasstructure.Services.Input;
using CodeBase.Infrasstructure.Services.PersistentProgress;
using CodeBase.Infrasstructure.Services.SaveLoad;
using CodeBase.Services.Randomizer;
using CodeBase.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
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
            RegisterStaticData();
            IRandomService randomService = new RandomService();
            _services.RegisterSingle(randomService);
            _services.RegisterSingle<IInputService>(InputService());
            _services.RegisterSingle<IAsset>(new AssetProvider());
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            
            _services.RegisterSingle<IUIFactory>(new UIFactory(_services.Single<IAsset>(),_services.Single<IStaticDataService>(),
                _services.Single<IPersistentProgressService>()));
            _services.RegisterSingle<IWindowService>(new WindowService(_services.Single<IUIFactory>()));

            _services.RegisterSingle<IGameFactory>(new GameFactory(
                _services.Single<IAsset>(),
                _services.Single<IStaticDataService>(),
                randomService,
                _services.Single<IPersistentProgressService>(),
                _services.Single<IWindowService>()));
            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistentProgressService>(), _services.Single<IGameFactory>()));
        }

        private void RegisterStaticData()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.Load();
            _services.RegisterSingle(staticData);
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