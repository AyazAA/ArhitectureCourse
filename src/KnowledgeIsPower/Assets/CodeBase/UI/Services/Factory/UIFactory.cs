using CodeBase.Infrasstructure.AssetManagement;
using CodeBase.Infrasstructure.Services;
using CodeBase.Infrasstructure.Services.PersistentProgress;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPath = "UI/UIRoot";
        private readonly IAsset _assets;
        private readonly IStaticDataService _staticData;
        private Transform _uiRoot;
        private readonly IPersistentProgressService _progressService;

        public UIFactory(IAsset assets, IStaticDataService staticData, IPersistentProgressService persistentProgressService)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = persistentProgressService;
        }

        public void CreateShop()
        {
            WindowConfig windowConfig = _staticData.ForWindow(WindowId.Shop);
            WindowBase window = Object.Instantiate(windowConfig.Prefab, _uiRoot);
            window.Construct(_progressService);
        }

        public void CreateUIRoot() => 
            _uiRoot = _assets.Instantiate(UIRootPath).transform;
    }
}