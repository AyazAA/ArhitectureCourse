﻿using CodeBase.Infrasstructure.Services;
using CodeBase.Infrasstructure.States;
using CodeBase.Logic;

namespace CodeBase.Infrasstructure
{
    public class Game
    {
        public GameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain curtain)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), curtain, AllServices.Container);
        }
    }
}