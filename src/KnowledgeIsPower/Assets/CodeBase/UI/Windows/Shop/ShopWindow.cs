using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;

namespace CodeBase.UI.Windows.Shop
{
    public class ShopWindow : WindowBase
    {
        public TextMeshProUGUI SkullText;
        public RewardedAdItem RewardedAdItem;

        public void Construct(IAdsService adsService, IPersistentProgressService progressService)
        {
            base.Construct(progressService);
            RewardedAdItem.Construct(adsService, progressService);
        }

        protected override void Initialize()
        {
            RefreshSkullText();
            RewardedAdItem.Initialize();
        }

        protected override void SubscribeUpdates()
        {
            Progress.WorldData.LootData.Changed += RefreshSkullText;
            RewardedAdItem.Subscribe();
        }

        protected override void CleanUp()
        {
            base.CleanUp();
            RewardedAdItem.CleanUp();
            Progress.WorldData.LootData.Changed -= RefreshSkullText;
        }

        private void RefreshSkullText() => 
            SkullText.text = Progress.WorldData.LootData.Collected.ToString();
    }
}