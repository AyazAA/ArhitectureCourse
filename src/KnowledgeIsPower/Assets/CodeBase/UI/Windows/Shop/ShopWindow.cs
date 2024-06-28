using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;
using Zenject;

namespace CodeBase.UI.Windows.Shop
{
    public class ShopWindow : WindowBase
    {
        public TextMeshProUGUI SkullText;
        public RewardedAdItem RewardedAdItem;
        public ShopItemsContainer ShopItemsContainer;

        [Inject]
        public void Construct(
            IAdsService adsService, 
            IPersistentProgressService progressService,
            IIAPService iapService, 
            IAsset assets)
        {
            base.Construct(progressService);
            RewardedAdItem.Construct(adsService, progressService);
            ShopItemsContainer.Construct(iapService, progressService, assets);
        }

        protected override void Initialize()
        {
            RefreshSkullText();
            RewardedAdItem.Initialize();
            ShopItemsContainer.Initialize();
        }

        protected override void SubscribeUpdates()
        {
            Progress.WorldData.LootData.Changed += RefreshSkullText;
            RewardedAdItem.Subscribe();
            ShopItemsContainer.Subscribe();
        }

        protected override void CleanUp()
        {
            base.CleanUp();
            RewardedAdItem.CleanUp();
            ShopItemsContainer.CleanUp();
            Progress.WorldData.LootData.Changed -= RefreshSkullText;
        }

        private void RefreshSkullText() => 
            SkullText.text = Progress.WorldData.LootData.Collected.ToString();
    }
}