using System;
using UnityEngine;
using Game.Code.Common;
using Game.Code.Infrastructure.Services.Inputs;
using Game.Code.Logic.Bombs;
using Game.Code.UI.Elements;

namespace Game.Code.Logic.Players
{
    public class Player : MonoBehaviour, IDamageble, ICoroutineRunner
    {
        public event Action Failured;
        
        [SerializeField] private Components _referencesComponents;
        [SerializeField] private MovementProps _movementProps;
        [SerializeField] private CombatProps _combatProps;

        private PlayerMovement _movement;
        private PlayerCombat _combat;

        private bool _disable;

        public void Init(IInputService inputService, BombSpawner bombSpawner)
        {
            _movement = new PlayerMovement(inputService, _referencesComponents, _movementProps);
            _movement.Init(transform, Camera.main);

            _combat = new PlayerCombat(inputService, bombSpawner, _combatProps, this);
            _combat.Init();
        }

        private void Update()
        {
            if (_disable == false)
            {
                _movement.Tick(Time.deltaTime);
                _combat.Tick(Time.deltaTime);
            }
        }

        public void ApplyDamage() => 
            Failured?.Invoke();

        public Transform GetTransform() => 
            transform;

        public void Disable() => 
            _disable = true;

        public CombatProps GetProps() => _combatProps;
    }

    [Serializable]
    public struct Components
    {
        public CharacterController Controller;
        public Animator PlayerAnimator;
    }
    
    [Serializable]
    public struct MovementProps
    {
        public float Speed;
    }
    
    [Serializable]
    public struct CombatProps
    {
        public BombReloadIndicator ReloadIndicator;
        public Transform BombDropPoint;
        public Transform BombContainer;
        public float Cooldown;
    }
}