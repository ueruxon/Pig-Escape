using UnityEngine;

namespace Game.Code.Infrastructure.Services.Inputs
{
    public class StandaloneInput : InputService
    {
        public override Vector2 Axis { get 
            {
                Vector2 axis = SimpleInputAxis();

                if (axis == Vector2.zero) {
                    return UnityAxis();
                }

                return axis;
            }
        }

        private static Vector2 UnityAxis() => 
            new Vector2(Input.GetAxis(Horizontal), Input.GetAxis(Vertical));
    }
}