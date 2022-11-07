﻿using Assets.CodeBase.Infrasstructure.Factory;
using CodeBase.CameraLogic;
using CodeBase.Logic;
using UnityEngine;

namespace Assets.CodeBase.Infrasstructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private const string InitialPointTag = "InitialPoint";
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadingCurtain _curtain;
        private readonly IGameFactory _gameFactory; 

        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain curtain, IGameFactory gameFactory)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _gameFactory = gameFactory;
        }

        public void Enter(string sceneName)
        {
            _curtain.Show();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit() =>
            _curtain.Hide();

        private void OnLoaded()
        {
            GameObject hero = _gameFactory.CreateHero(GameObject.FindWithTag(InitialPointTag));
            _gameFactory.CreateHud();
            CameraFollowInit(hero);

            _stateMachine.Enter<GameLoopState>();
        }

        private void CameraFollowInit(GameObject target)
        {
            Camera.main.GetComponent<CameraFollow>().Follow(target);
        }
    }
}