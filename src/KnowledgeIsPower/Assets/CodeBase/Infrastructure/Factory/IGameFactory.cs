using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        public Task<GameObject> CreateHero(Vector3 at);
        public Task<GameObject> CreateHud();
        Task CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId);
        void Cleanup();
        Task<GameObject> CreateMonster(MonsterTypeId monsterTypeId, Transform parent);
        Task<LootPiece> CreateLoot();
        Task WarmUp();
    }
}
