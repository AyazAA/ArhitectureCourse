using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.Randomizer;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAsset _asset;
        private readonly IStaticDataService _staticDataService;
        private readonly IRandomService _randomService;
        private readonly IPersistentProgressService _progressService;
        private readonly IWindowService _windowService;
        public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
        public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
        private GameObject HeroGameObject { get; set; }

        public GameFactory(IAsset asset, IStaticDataService staticDataService, IRandomService randomService, IPersistentProgressService progressService, IWindowService windowService)
        {
            _asset = asset;
            _staticDataService = staticDataService;
            _randomService = randomService;
            _progressService = progressService;
            _windowService = windowService;
        }

        public async Task WarmUp()
        {
            await _asset.Load<GameObject>(AssetAddress.Loot);
            await _asset.Load<GameObject>(AssetAddress.Spawner);
        }

        public async Task<GameObject> CreateHero(Vector3 at)
        {
            HeroGameObject = await InstantiateRegisteredAsync(AssetAddress.HeroPath, at);
            return HeroGameObject;
        }

        public async Task<GameObject> CreateHud()
        {
            GameObject hud = await InstantiateRegisteredAsync(AssetAddress.HudPath);
            hud.GetComponentInChildren<LootCounter>().Construct(_progressService.Progress.WorldData);

            foreach (OpenWindowButton button in hud.GetComponentsInChildren<OpenWindowButton>())
                button.Construct(_windowService);

            return hud;
        }

        public async Task CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId)
        {
            GameObject prefab = await _asset.Load<GameObject>(AssetAddress.Spawner);

            SpawnPoint spawner = InstantiateRegistered(prefab, at)
                 .GetComponent<SpawnPoint>();
             spawner.Construct(this);
             spawner.MonsterTypeId = monsterTypeId;
             spawner.Id = spawnerId;
        }

        public async Task<LootPiece> CreateLoot()
        {
            GameObject prefab = await _asset.Load<GameObject>(AssetAddress.Loot);
            
            LootPiece lootPiece = InstantiateRegistered(prefab).GetComponent<LootPiece>();
            lootPiece.Construct(_progressService.Progress.WorldData);
            return lootPiece;
        }

        public async Task<GameObject> CreateMonster(MonsterTypeId monsterTypeId, Transform parent)
        {
            MonsterStaticData monsterData = _staticDataService.ForMonster(monsterTypeId);


            GameObject prefab = await _asset.Load<GameObject>(monsterData.PrefabReference);
            GameObject monster = GameObject.Instantiate(prefab, parent.position, Quaternion.identity, parent);

            var health = monster.GetComponent<IHealth>();
            health.Current = monsterData.Hp;
            health.Max= monsterData.Hp;
            
            monster.GetComponent<ActorUI>().Construct(health);
            monster.GetComponent<AgentMoveToHero>().Construct(HeroGameObject.transform);
            monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

            var attack = monster.GetComponent<Attack>();
            attack.Construct(HeroGameObject.transform);
            attack.Damage = monsterData.Damage;
            attack.Cleavage = monsterData.Cleavage;
            attack.EffectiveDistance = monsterData.EffectiveDistance;

            var lootSpawner = monster.GetComponentInChildren<LootSpawner>();
            lootSpawner.SetLoot(monsterData.MinLoot, monsterData.MaxLoot);
            lootSpawner.Construct(this, _randomService);
            
            monster.GetComponent<RotateToHero>()?.Construct(HeroGameObject.transform);
            
            return monster;
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
            
            _asset.CleanUp();
        }

        public void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress writer)
                ProgressWriters.Add(writer);
            ProgressReaders.Add(progressReader);
        }

        private GameObject InstantiateRegistered(GameObject prefab, Vector3 position)
        {
            GameObject gameObject = Object.Instantiate(prefab,  position, Quaternion.identity);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(GameObject prefab)
        {
            GameObject gameObject = Object.Instantiate(prefab);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 position)
        {
            GameObject gameObject = await _asset.Instantiate(prefabPath, at: position);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
        {
            GameObject gameObject = await _asset.Instantiate(prefabPath);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (var reader in gameObject.GetComponentsInChildren<ISavedProgressReader>()) 
                Register(reader);
        }
    }
}