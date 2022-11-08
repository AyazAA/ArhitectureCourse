using UnityEngine;

namespace CodeBase.Infrasstructure.Services.Input
{
    public class MobileInputService : InputSevice
    {
        public override Vector2 Axis => SimpleInputAxis();
    }
}