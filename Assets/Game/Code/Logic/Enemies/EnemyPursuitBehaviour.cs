using System;
using Game.Code.Common;
using UnityEngine;

namespace Game.Code.Logic.Enemies
{
    public class EnemyPursuitBehaviour : ITickable
    {
        private const string Speed = "Speed";
        private static readonly int SpeedHash = Animator.StringToHash(Speed);
        
        public event Action<bool> PursuitEnded;
        
        private readonly Components _components;
        private readonly PursuitProps _pursuitProps;

        private Transform _myTransform;
        private Transform _targetTransform;
        
        private Vector3 _lastTargetPosition;

        private float _currentWaitingTimer;
        private bool _inPursuit;
        private bool _alreadyMove;
        
        public EnemyPursuitBehaviour(Components components, PursuitProps pursuitProps)
        {
            _components = components;
            _pursuitProps = pursuitProps;
        }

        public void Init(Transform myTransform, Transform targetTransform)
        {
            _targetTransform = targetTransform;
            _myTransform = myTransform;
        }

        public void Start(Vector3 lastTargetPosition)
        {
            _components.DetectionIndicator.Show();
            _components.Agent.enabled = false;
            _components.Agent.speed = _pursuitProps.PursuitSpeed;
            _lastTargetPosition = lastTargetPosition;
            
            _inPursuit = false;
        }


        public void Tick(float deltaTime)
        {
            _components.EnemyAnimator.SetFloat(SpeedHash, _components.Agent.velocity.magnitude);
            
            if (_inPursuit == false)
                WaitBeforePursuit(deltaTime);

            if (_inPursuit)
                PursuitTarget();
        }

        private void PursuitTarget()
        {
            if (_alreadyMove)
            {
                if (!_components.Agent.pathPending && _components.Agent.remainingDistance < .3f)
                {
                    _alreadyMove = false;
                    _inPursuit = false;

                    float distanceToTarget = Vector3.Distance(_targetTransform.position, _myTransform.position);
                    
                    if (distanceToTarget < _pursuitProps.CaptureDistance)
                    {
                        PursuitEnded?.Invoke(true);
                    }
                    else
                        PursuitEnded?.Invoke(false);
                }
            }
            else
            {
                _components.Agent.SetDestination(_lastTargetPosition);
                _alreadyMove = true;
            }
        }

        private void WaitBeforePursuit(float deltaTime)
        {
            _currentWaitingTimer += deltaTime;
            
            if (_currentWaitingTimer >= _pursuitProps.DelayBeforePursuit)
            {
                _components.Agent.enabled = true;
                _inPursuit = true;
                
                _currentWaitingTimer = 0;
            }
        }
    }
}