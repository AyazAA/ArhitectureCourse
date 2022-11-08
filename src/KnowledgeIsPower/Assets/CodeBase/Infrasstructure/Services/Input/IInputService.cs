using UnityEngine;

namespace CodeBase.Infrasstructure.Services.Input
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