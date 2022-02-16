using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Code.Common;
using Game.Code.UI.Elements;

public enum EnemyState
{
    None,
    Patrol,
    Pursuit,
}

namespace Game.Code.Logic.Enemies
{
    public abstract class Enemy : MonoBehaviour, IDamageble
    {
        [SerializeField] protected Components _components;
        [SerializeField] protected MovementProps _movementProperties;
        [SerializeField] protected DetectionProps _detectionProperties;
        [SerializeField] protected PursuitProps _pursuitProperties;
        
        private const string Dead = "Dead";
        private const string Idle = "Idle";
        private static readonly int DeadHash = Animator.StringToHash(Dead);
        private static readonly int IdleHash = Animator.StringToHash(Idle);

        private EnemyPatrolBehaviour _patrolBehaviourBehaviour;
        private EnemyFindTargetBehaviour _findTargetBehaviour;
        private EnemyPursuitBehaviour _pursuitBehaviour;

        private IDamageble _player;
        private bool _isDead;

        protected EnemyState _state;
        
        public virtual void Init(List<Transform> waypointList, IDamageble player)
        {
            _player = player;
            
            WaypointNetwork waypointNetwork = new WaypointNetwork(waypointList);
            _patrolBehaviourBehaviour = new EnemyPatrolBehaviour(_components, _movementProperties, waypointNetwork);
            _patrolBehaviourBehaviour.Init();

            _findTargetBehaviour = new EnemyFindTargetBehaviour(_components, _detectionProperties);
            _findTargetBehaviour.Init(transform, player.GetTransform());
            _findTargetBehaviour.Detected += OnTargetDetected;

            _pursuitBehaviour = new EnemyPursuitBehaviour(_components, _pursuitProperties);
            _pursuitBehaviour.Init(transform, player.GetTransform());
            _pursuitBehaviour.PursuitEnded += OnPursuitEnded;

            SetState(EnemyState.Patrol);
        }

        public virtual void Update()
        {
            if (_isDead == false)
            {
                switch (_state)
                {
                    case EnemyState.None:
                        break;
                    case EnemyState.Patrol:
                        _patrolBehaviourBehaviour.Tick(Time.deltaTime);
                        _findTargetBehaviour.Tick(Time.deltaTime);
                        break;
                    case EnemyState.Pursuit:
                        _pursuitBehaviour.Tick(Time.deltaTime);
                        break;
                }
            }
        }

        public void ApplyDamage()
        {
            if (_isDead == false) 
                OnDead();
        }

        public Transform GetTransform() => 
            transform;

        private void SetState(EnemyState nextState) => 
            _state = nextState;

        private void OnTargetDetected()
        {
            SetState(EnemyState.Pursuit);
            
            _components.DetectionZone.Hide();
            _pursuitBehaviour.Start(_player.GetTransform().position);
        }

        private void OnPursuitEnded(bool capture)
        {
            _components.DetectionIndicator.Hide();
            _components.DetectionZone.Show();
            _components.Agent.speed = _movementProperties.Speed;

            if (capture)
            {
                SetState(EnemyState.None);
                _player.ApplyDamage();
            }
            else
                SetState(EnemyState.Patrol);
        }
        
        private void OnDead()
        {
            _isDead = true;
            Disable();
        }

        public void Disable()
        {
            SetState(EnemyState.None);
            
            _components.Agent.enabled = false;
            _components.DetectionZone.Hide();
            _components.DetectionIndicator.Hide();
            
            if (_isDead)
                _components.EnemyAnimator.SetTrigger(DeadHash);
            else
                _components.EnemyAnimator.SetBool(IdleHash, true);
        }
    }
    
    [System.Serializable]
    public struct Components
    {
        public NavMeshAgent Agent;
        public Animator EnemyAnimator;
        public DetectionZone DetectionZone;
        public DetectionIndicator DetectionIndicator;
    }
    
    [System.Serializable]
    public struct MovementProps
    {
        public float Speed;
        public float RemainingDistance;
        public bool RandomMovement;
    }

    [System.Serializable]
    public struct DetectionProps
    {
        public float DetectionRadius;
        public float DetectionAngle;
    }
    
    [System.Serializable]
    public struct PursuitProps
    {
        public float PursuitSpeed;
        public float CaptureDistance;
        public float DelayBeforePursuit;
    }
}