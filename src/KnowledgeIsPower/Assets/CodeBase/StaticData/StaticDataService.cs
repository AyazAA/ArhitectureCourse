﻿using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrasstructure.Services;
using UnityEngine;

namespace CodeBase.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;

        public void LoadMonsters()
        {
            _monsters = Resources.LoadAll<MonsterStaticData>("StaticData/Monsters")
                .ToDictionary(x => x.MonsterTypeId, x => x);
        }

        public MonsterStaticData ForMonster(MonsterTypeId typeId) => 
            _monsters.TryGetValue(typeId, out MonsterStaticData staticData) 
                ? staticData 
                : null;
    }
}