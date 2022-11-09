using CodeBase.Infrasstructure.Factory;
using CodeBase.Infrasstructure.Services;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy
{
    public class AgentMoveToHero: Follow
    {
        private const float MinimalDistance = 1;
        [SerializeField] private NavMeshAgent Agent;
        private Transform _heroTransform;
        private IGameFactory _gameFactory;


        private void Awake()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            if (_gameFactory.HeroGameObject != null)
                InitializeHeroTransform();
            else
                _gameFactory.HeroCreated += OnHeroCreated;
        }

        private void Update()
        {
            if(Initialized() && HeroNotReached())
                Agent.destination = _heroTransform.position;
        }

        private void InitializeHeroTransform() =>
            _heroTransform = _gameFactory.HeroGameObject.transform;

        private void OnHeroCreated() => 
            InitializeHeroTransform();

        private bool Initialized() => 
            _heroTransform != null;

        private bool HeroNotReached() => 
            Vector3.Distance(Agent.transform.position, _heroTransform.position)>= MinimalDistance;
    }
}