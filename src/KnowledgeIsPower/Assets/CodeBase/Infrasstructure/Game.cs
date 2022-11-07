using Assets.CodeBase.Infrasstructure.Services;
using Assets.CodeBase.Infrasstructure.States;
using CodeBase.Logic;

namespace Assets.CodeBase.Infrasstructure
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