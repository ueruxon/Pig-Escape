using Game.Code.Common;
using UnityEngine;

namespace Game.Code.Logic.Enemies
{
    public class EnemyPatrolBehaviour : ITickable
    {
        private const string Speed = "Speed";
        private static readonly int SpeedHash = Animator.StringToHash(Speed);
        
        private enum State {
            PatrollingMovement,
            PatrollingIdle,
        }

        private readonly Components _components;
        private readonly MovementProps _movementProps;
        private readonly WaypointNetwork _waypointNetwork;

        private State _currentState;
        private float _patrolTimer;
        private bool _alreadyMove;

        public EnemyPatrolBehaviour(Components components, MovementProps movementProps, WaypointNetwork waypointNetwork)
        {
            _components = components;
            _movementProps = movementProps;
            _waypointNetwork = waypointNetwork;
        }

        public void Init()
        {
            _currentState = State.PatrollingIdle;
            _components.Agent.speed = _movementProps.Speed;
            _patrolTimer = Random.Range(2f, 4f);
        }

        public void Tick(float deltaTime)
        {
            _components.EnemyAnimator.SetFloat(SpeedHash, _components.Agent.velocity.magnitude);
            
            switch (_currentState) {
                case State.PatrollingMovement:
                    Move(_movementProps.RemainingDistance);
                    break;;
                case State.PatrollingIdle:
                    Idle(deltaTime);
                    break;
            }
        }

        private void Idle(float deltaTime)
        {
            _patrolTimer -= deltaTime;
            
            if (_patrolTimer < 0f)
            {
                _patrolTimer = Random.Range(2f, 4f);

                TryToSetNextDestination();
                _currentState = State.PatrollingMovement;
            }
        }

        private void Move(float remainingDistance)
        {
            if (_components.Agent.enabled == false)
                _components.Agent.enabled = true;
            
            if (_alreadyMove)
            {
                if (!_components.Agent.pathPending && _components.Agent.remainingDistance < remainingDistance)
                {
                    _alreadyMove = false;
                    _currentState = State.PatrollingIdle;
                }
            }
        }

        private bool TryToSetNextDestination()
        {
            Transform destination = null;

            destination = _movementProps.RandomMovement 
                ? _waypointNetwork.GetRandomWaypoint() 
                : _waypointNetwork.GetNextWaypoint();

            if (destination == null)
                return false;

            _components.Agent.SetDestination(destination.position);
            _alreadyMove = true;
            return true;
        }
    
    }
}