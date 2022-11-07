using Assets.CodeBase.Infrasstructure.Services;
using UnityEngine;

namespace Assets.CodeBase.Infrasstructure.Factory
{
    public interface IGameFactory : IService
    {
        public GameObject CreateHero(GameObject at);
        public void CreateHud();
    }
}
