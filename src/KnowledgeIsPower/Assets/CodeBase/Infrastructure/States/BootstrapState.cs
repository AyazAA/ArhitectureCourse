using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.Randomizer;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Infrastructure.States
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

            RegisterAdsService();

            _services.RegisterSingle<IGameStateMachine>(_stateMachine);
            IRandomService randomService = new RandomService();
            _services.RegisterSingle(randomService);
            _services.RegisterSingle<IInputService>(InputService());
            RegisterAssetProvider();
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            
            RegisterIAPService(new IAPProvider(),_services.Single<IPersistentProgressService>());

            _services.RegisterSingle<IUIFactory>(new UIFactory(
                _services.Single<IAsset>(),
                _services.Single<IStaticDataService>(),
                _services.Single<IPersistentProgressService>(),
                _services.Single<IAdsService>(),
                _services.Single<IIAPService>()
                ));
            
            _services.RegisterSingle<IWindowService>(new WindowService(_services.Single<IUIFactory>()));

            _services.RegisterSingle<IGameFactory>(new GameFactory(
                _services.Single<IAsset>(),
                _services.Single<IStaticDataService>(),
                randomService,
                _services.Single<IPersistentProgressService>(),
                _services.Single<IWindowService>()));
            _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(_services.Single<IPersistentProgressService>(), _services.Single<IGameFactory>()));
        }

        private void RegisterAssetProvider()
        {
            IAsset assetProvider = new AssetProvider();
            assetProvider.Initialize();
            _services.RegisterSingle<IAsset>(assetProvider);
        }

        private void RegisterStaticData()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.Load();
            _services.RegisterSingle(staticData);
        }

        private void RegisterAdsService()
        {
            var adsService = new AdsService();
            adsService.Initialize();
            _services.RegisterSingle<IAdsService>(adsService);
        }
        private void RegisterIAPService(IAPProvider iapProvider, IPersistentProgressService progressService)
        {
            IAPService iapService = new IAPService(iapProvider, progressService);
            iapService.Initialize();
            _services.RegisterSingle<IIAPService>(iapService);
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