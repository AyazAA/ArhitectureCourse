﻿using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrasstructure.Factory;
using CodeBase.Infrasstructure.Services;
using CodeBase.Infrasstructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic
{
    public class EnemySpawner : MonoBehaviour, ISavedProgress
    {
        public MonsterTypeId MonsterTypeId;
        public bool Slain => _slain;
        [SerializeField] private bool _slain;
        private string _id;
        private IGameFactory _factory;
        private EnemyDeath _enemyDeath;


        private void Awake()
        {
            _id = GetComponent<UniqueId>().Id;
            _factory = AllServices.Container.Single<IGameFactory>();
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(_id))
                _slain = true;
            else
                Spawn();
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (Slain)
                progress.KillData.ClearedSpawners.Add(_id);
        }

        private void Spawn()
        {
            GameObject monster = _factory.CreateMonster(MonsterTypeId, transform);
            _enemyDeath = monster.GetComponent<EnemyDeath>();
            _enemyDeath.Happened += Slay;
        }

        private void Slay()
        {
            if (_enemyDeath != null)
                _enemyDeath.Happened -= Slay;
            
            _slain = true;
        }
    }
}