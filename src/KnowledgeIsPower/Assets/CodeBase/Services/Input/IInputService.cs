using Assets.CodeBase.Infrasstructure.Services;
using UnityEngine;

namespace Assets.CodeBase.Services.Input
{
    public interface IInputService : IService
    {
        Vector2 Axis
        {
            get;
        }
        bool IsAttackButtonUp();
    }
}