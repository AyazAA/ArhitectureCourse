using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using CodeBase.Enemy;
using CodeBase.Infrasstructure.AssetManagement;
using CodeBase.Infrasstructure.Services;
using CodeBase.Infrasstructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Services.Randomizer;
using CodeBase.StaticData;
using CodeBase.UI;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Infrasstructure.Factory
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

        public GameObject CreateHero(GameObject at)
        {
            HeroGameObject = InstantiateRegistered(AssetPath.HeroPath, at.transform.position);
            return HeroGameObject;
        }

        public GameObject CreateHud()
        {
            GameObject hud = InstantiateRegistered(AssetPath.HudPath);
            hud.GetComponentInChildren<LootCounter>().Construct(_progressService.Progress.WorldData);

            foreach (OpenWindowButton button in hud.GetComponentsInChildren<OpenWindowButton>())
                button.Construct(_windowService);

            return hud;
        }

        public void CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId)
        {
             SpawnPoint spawner = InstantiateRegistered(AssetPath.Spawner, at)
                 .GetComponent<SpawnPoint>();
             spawner.Construct(this);
             spawner.MonsterTypeId = monsterTypeId;
             spawner.Id = spawnerId;
        }

        public GameObject CreateMonster(MonsterTypeId monsterTypeId, Transform parent)
        {
            MonsterStaticData monsterData = _staticDataService.ForMonster(monsterTypeId);
            GameObject monster = GameObject.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);

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

        public LootPiece CreateLoot()
        {
            LootPiece lootPiece = InstantiateRegistered(AssetPath.LootPath).GetComponent<LootPiece>();
            lootPiece.Construct(_progressService.Progress.WorldData);
            return lootPiece;
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }

        public void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress writer)
                ProgressWriters.Add(writer);
            ProgressReaders.Add(progressReader);
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 position)
        {
            GameObject gameObject = _asset.Instantiate(prefabPath, at: position);
            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private GameObject InstantiateRegistered(string prefabPath)
        {
            GameObject gameObject = _asset.Instantiate(prefabPath);
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