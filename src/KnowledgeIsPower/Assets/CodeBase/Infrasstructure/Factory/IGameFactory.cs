using System;
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
        GameObject HeroGameObject { get; }
        event Action HeroCreated;
        public GameObject CreateHero(GameObject at);
        public GameObject CreateHud();
        void Cleanup();
        void Register(ISavedProgressReader progressReader);
    }
}
