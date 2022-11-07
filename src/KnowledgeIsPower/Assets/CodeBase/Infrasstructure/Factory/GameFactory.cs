using Assets.CodeBase.Infrasstructure.AssetManagement;
using UnityEngine;

namespace Assets.CodeBase.Infrasstructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAsset _asset;

        public GameFactory(IAsset asset)
        {
            _asset = asset;
        }

        public GameObject CreateHero(GameObject at) =>
            _asset.Instantiate(AssetPath.HeroPath, at: at.transform.position);

        public void CreateHud() =>
            _asset.Instantiate(AssetPath.HudPath);
    }
}