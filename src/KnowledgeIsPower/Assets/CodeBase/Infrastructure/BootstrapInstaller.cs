using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.Randomizer;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.States;
using CodeBase.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindStaticData();
            BindAdsService();
            BindRandomService();
            BindInputService();
            BindAssetProvider();
            BindPersistentProgressService();
            BindIAPProvider();
            BindIAPService();
            BindUIFactory();
            BindWindowService();
            BindGameFactory();
            BindSaveLoadService();
            BindGameStateMachine();
        }

        private void BindGameStateMachine()
        {
            Container
                .Bind<IGameStateMachine>()
                .To<GameStateMachine>()
                .AsSingle();
        }

        private void BindSaveLoadService()
        {
            Container
                .Bind<ISaveLoadService>()
                .To<SaveLoadService>()
                .AsSingle();
        }

        private ConcreteIdArgConditionCopyNonLazyBinder BindGameFactory()
        {
            return Container
                .Bind<IGameFactory>()
                .To<GameFactory>()
                .AsSingle();
        }

        private ConcreteIdArgConditionCopyNonLazyBinder BindWindowService()
        {
            return Container
                .Bind<IWindowService>()
                .To<WindowService>()
                .AsSingle();
        }

        private ConcreteIdArgConditionCopyNonLazyBinder BindUIFactory()
        {
            return Container
                .Bind<IUIFactory>()
                .To<UIFactory>()
                .AsSingle();
        }

        private void BindIAPService()
        {
            Container
                .Bind<IIAPService>()
                .To<IAPService>()
                .AsSingle();
        }

        private void BindIAPProvider()
        {
            var iapProvider = new IAPProvider();
            Container
                .Bind<IAPProvider>()
                .FromInstance(iapProvider)
                .AsSingle();
        }

        private void BindPersistentProgressService()
        {
            Container
                .Bind<IPersistentProgressService>()
                .To<PersistentProgressService>()
                .AsSingle();
        }

        private void BindAssetProvider()
        {
            IAsset assetProvider = new AssetProvider();
            assetProvider.Initialize();
            Container
                .Bind<IAsset>()
                .FromInstance(assetProvider)
                .AsSingle();
        }

        private void BindInputService()
        {
            IInputService inputService;
            if (Application.isEditor)
                inputService = new StandoleInputService();
            else
                inputService = new MobileInputService();

            Container
                .Bind<IInputService>()
                .FromInstance(inputService)
                .AsSingle();
        }

        private void BindRandomService()
        {
            Container
                .Bind<IRandomService>()
                .To<RandomService>()
                .AsSingle();
        }

        private void BindStaticData()
        {
            IStaticDataService staticData = new StaticDataService();
            staticData.Load();
            Container
                .Bind<IStaticDataService>()
                .FromInstance(staticData)
                .AsSingle();
        }

        private void BindAdsService()
        {
            var adsService = new AdsService();
            //adsService.Initialize();
            Container.Bind<IAdsService>().FromInstance(adsService).AsSingle();
        }
    }
}