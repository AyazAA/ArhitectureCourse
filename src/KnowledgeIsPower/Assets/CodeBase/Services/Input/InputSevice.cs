﻿using UnityEngine;

namespace Assets.CodeBase.Services.Input
{
    public abstract class InputSevice : IInputService
    {
        protected const string Horizontal = "Horizontal";
        protected const string Vertical = "Vertical";
        private const string Button = "Fire";

        public abstract Vector2 Axis
        {
            get;
        }

        protected static Vector2 SimpleInputAxis() =>
            new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));

        public bool IsAttackButtonUp() =>
            SimpleInput.GetButtonUp(Button);
    }
}