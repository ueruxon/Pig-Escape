using UnityEngine;

namespace Game.Code.Infrastructure.Services.Inputs
{
    public abstract class InputService : IInputService {
        protected const string Horizontal = "Horizontal";
        protected const string Vertical = "Vertical";
        protected const string Fire = "FireBomb";

        public abstract Vector2 Axis { get; }

        protected static Vector2 SimpleInputAxis() => 
            new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));

        public bool AttackButtonUp() => SimpleInput.GetButtonUp(Fire);
    }
}