using Assets.CodeBase.Infrasstructure.Services;
using UnityEngine;

namespace Assets.CodeBase.Infrasstructure.AssetManagement
{
    public interface IAsset : IService
    {
        public GameObject Instantiate(string path);
        public GameObject Instantiate(string path, Vector3 at);
    }
}