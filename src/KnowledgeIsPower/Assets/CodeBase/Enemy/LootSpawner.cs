using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.Randomizer;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class LootSpawner : MonoBehaviour
    {
        [SerializeField] private EnemyDeath _enemyDeath;
        private IGameFactory _factory;
        private IRandomService _randomService;
        private int _lootMin;
        private int _lootMax;

        public void Construct(IGameFactory gameFactory, IRandomService randomService)
        {
            _factory = gameFactory;
            _randomService = randomService;
        }

        private void OnEnable() => 
            _enemyDeath.Happened += SpawnLoot;

        private void OnDisable() => 
            _enemyDeath.Happened -= SpawnLoot;

        private async void SpawnLoot()
        {
            LootPiece lootPiece = await _factory.CreateLoot();
            lootPiece.transform.position = transform.position;
            Loot lootItem = GenerateLoot();
            lootPiece.Initialize(lootItem);
        }

        private Loot GenerateLoot()
        {
            return new Loot
            {
                Value = _randomService.Next(_lootMin, _lootMax)   
            };
        }

        public void SetLoot(int min, int max)
        {
            _lootMin = min;
            _lootMax = max;
        }
    }
}