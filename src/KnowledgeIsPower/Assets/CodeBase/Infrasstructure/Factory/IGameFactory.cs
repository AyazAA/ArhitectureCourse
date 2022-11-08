using System.Collections.Generic;
using CodeBase.Infrasstructure.Services;
using CodeBase.Infrasstructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrasstructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        public GameObject CreateHero(GameObject at);
        public void CreateHud();
        void Cleanup();
    }
}
