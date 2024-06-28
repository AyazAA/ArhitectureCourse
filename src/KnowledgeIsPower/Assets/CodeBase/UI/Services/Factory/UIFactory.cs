using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using CodeBase.UI.Windows.Shop;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Services.Factory
{
    public class UIFactory : IUIFactory
    {
        private const string UIRootPath = "UIRoot";
        private readonly IAsset _assets;
        private readonly IStaticDataService _staticData;
        private Transform _uiRoot;
        private readonly IPersistentProgressService _progressService;
        private readonly IAdsService _adsService;
        private readonly IIAPService _iapService;

        [Inject]
        public UIFactory(IAsset assets,
            IStaticDataService staticData,
            IPersistentProgressService persistentProgressService,
            IAdsService adsService, 
            IIAPService iapService)
        {
            _assets = assets;
            _staticData = staticData;
            _progressService = persistentProgressService;
            _adsService = adsService;
            _iapService = iapService;
        }

        public void CreateShop()
        {
            WindowConfig windowConfig = _staticData.ForWindow(WindowId.Shop);
            ShopWindow window = Object.Instantiate(windowConfig.Prefab, _uiRoot) as ShopWindow;
            window.Construct(_adsService, _progressService, _iapService, _assets);
        }

        public async Task CreateUIRoot()
        {
            GameObject root = await _assets.Instantiate(UIRootPath);
            _uiRoot = root.transform;
        }
    }
}