using UnityEngine;

namespace Game.Code.Infrastructure.Services.Inputs
{
    public class MobileInput : InputService
    {
        public override Vector2 Axis => SimpleInputAxis();
    }
}