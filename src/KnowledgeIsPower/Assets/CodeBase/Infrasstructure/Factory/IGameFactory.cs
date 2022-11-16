using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Infrasstructure.Services;
using CodeBase.Infrasstructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrasstructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        public GameObject CreateHero(GameObject at);
        public GameObject CreateHud();
        void CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);
        void Cleanup();
        GameObject CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
        LootPiece CreateLoot();
    }
}
