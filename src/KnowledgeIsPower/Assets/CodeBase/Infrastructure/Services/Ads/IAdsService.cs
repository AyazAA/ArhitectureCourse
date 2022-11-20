using System;

namespace CodeBase.Infrastructure.Services.Ads
{
    public interface IAdsService : IService
    {
        event Action RewardedVideoReady;
        int Reward { get; }
        void Initialize();
        bool IsRewardedVideoReady();
        void ShowRewardedVideo(Action onVideoFinished);
    }
}