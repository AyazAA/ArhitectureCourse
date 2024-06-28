using System;
using UnityEngine;
using UnityEngine.Advertisements;
using Application = UnityEngine.Device.Application;

namespace CodeBase.Infrastructure.Services.Ads
{
    public class AdsService : IAdsService
    {
        public event Action RewardedVideoReady;
        
        private const string AndroidGameId = "5024985";
        private const string IOSGameId = "5024984";
        private const string RewardedVideoPlacementId = "Rewarded_iOS";
        
        private string _gameId;
        private Action _onVideoFinished;

        public int Reward => 13;

        public void Initialize()
        {
            SetGameId();
            
            Advertisement.Initialize(_gameId);
        }

        public bool IsRewardedVideoReady()
        {
            throw new NotImplementedException();
        }

        public void ShowRewardedVideo(Action onVideoFinished)
        {
            Advertisement.Show(RewardedVideoPlacementId);
            _onVideoFinished = onVideoFinished;
        }

        public void OnUnityAdsReady(string placementId)
        {
            Debug.Log($"OnUnityAdsReady {placementId}");

            if (placementId == RewardedVideoPlacementId)
                RewardedVideoReady?.Invoke();
        }

        public void OnUnityAdsDidError(string message)
        {
            Debug.Log($"OnUnityAdsDidError {message}");
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            Debug.Log($"OnUnityAdsDidStart {placementId}");
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            switch (showResult)
            {
                case ShowResult.Failed:
                    Debug.LogError($"OnUnityAdsDidFinish {showResult}");
                    break;
                case ShowResult.Skipped:
                    Debug.LogError($"OnUnityAdsDidFinish {showResult}");
                    break;
                case ShowResult.Finished:
                    _onVideoFinished?.Invoke();    
                    break;
                default:
                    Debug.LogError($"OnUnityAdsDidFinish {showResult}");
                    break;
            }

            _onVideoFinished = null;
        }

        private void SetGameId()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _gameId = AndroidGameId;
                    break;
                case RuntimePlatform.IPhonePlayer:
                    _gameId = IOSGameId;
                    break;
                case RuntimePlatform.WindowsEditor:
                    _gameId = IOSGameId;
                    break;
                default:
                    Debug.Log("Unsupported platform for ads");
                    break;
            }
        }
    }
}