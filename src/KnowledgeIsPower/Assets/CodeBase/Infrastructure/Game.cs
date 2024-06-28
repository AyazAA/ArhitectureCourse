using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;
using Zenject;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public GameStateMachine StateMachine;

        public Game(IGameStateMachine gameStateMachine)
        {
            StateMachine = (GameStateMachine)gameStateMachine;
        }

        public void InitializeStateMachine(ICoroutineRunner coroutineRunner, LoadingCurtain curtain)
        {
            StateMachine.SetStates(new SceneLoader(coroutineRunner), curtain);
        }
    }
}