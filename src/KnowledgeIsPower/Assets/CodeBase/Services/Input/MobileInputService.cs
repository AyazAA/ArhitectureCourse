using UnityEngine;

namespace Assets.CodeBase.Services.Input
{
    public class MobileInputService : InputSevice
    {
        public override Vector2 Axis => SimpleInputAxis();
    }
}