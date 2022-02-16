using Game.Code.Common;
using Game.Code.Infrastructure.Services.Inputs;
using UnityEngine;

namespace Game.Code.Logic.Players
{
    public class PlayerMovement : ITickable
    {
        private const string MovementSpeed = "Speed";
        private const float Epsilon = 0.001f;
        private readonly int Speed = Animator.StringToHash(MovementSpeed);

        private readonly IInputService _inputService;

        private readonly Components _components;
        private readonly MovementProps _props;

        private Transform _playerTransform;
        private Camera _camera;

        public PlayerMovement(IInputService inputService, Components referencesComponents, MovementProps movementProps)
        {
            _inputService = inputService;
            _components = referencesComponents;
            _props = movementProps;
        }

        public void Init(Transform transform, Camera camera) {
            _playerTransform = transform;
            _camera = camera;
        }
        

        public void Tick(float deltaTime)
        {
            Vector3 movementVector = Vector3.zero;

            if (_inputService.Axis.sqrMagnitude > Epsilon)
            {
                movementVector = _camera.transform.TransformDirection(_inputService.Axis);
                movementVector.y = 0f;
                movementVector.Normalize();

                _playerTransform.forward = movementVector;
            }

            movementVector += Physics.gravity;

            _components.Controller.Move(movementVector * _props.Speed * deltaTime);
            _components.PlayerAnimator.SetFloat(Speed, _components.Controller.velocity.magnitude);
        }
    }
}