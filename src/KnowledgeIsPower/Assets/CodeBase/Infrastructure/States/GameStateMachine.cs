using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Logic;
using CodeBase.UI.Services.Factory;
using Zenject;

namespace CodeBase.Infrastructure.States
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly IGameFactory _gameFactory;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IStaticDataService _staticDataService;
        private readonly IUIFactory _uiFactory;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IAdsService _adsService;
        private Dictionary<Type, IExitableState> _states;
        private IExitableState _currentState;

        [Inject]
        public GameStateMachine(IGameFactory gameFactory,
            IPersistentProgressService persistentProgressService, IStaticDataService staticDataService,
            IUIFactory uiFactory, ISaveLoadService saveLoadService, IAdsService adsService)
        {
            _gameFactory = gameFactory;
            _persistentProgressService = persistentProgressService;
            _staticDataService = staticDataService;
            _uiFactory = uiFactory;
            _saveLoadService = saveLoadService;
            _adsService = adsService;
        }

        public void SetStates(SceneLoader sceneLoader, LoadingCurtain curtain)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, _adsService),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader, curtain, _gameFactory,
                    _persistentProgressService, _staticDataService, _uiFactory),
                [typeof(LoadProgressState)] = new LoadProgressState(this,_persistentProgressService,_saveLoadService),
                [typeof(GameLoopState)] = new GameLoopState(this),
            };
        }
        

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            TState state = GetState<TState>();
            _currentState?.Exit();
            _currentState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}