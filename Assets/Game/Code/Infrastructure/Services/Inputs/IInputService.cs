using UnityEngine;

namespace Game.Code.Infrastructure.Services.Inputs
{
    public interface IInputService : IService
    {
        Vector2 Axis { get; }
        bool AttackButtonUp();
    }
}