using System;
using Game.Code.Common;
using UnityEngine;

namespace Game.Code.Logic.Enemies
{
    public class EnemyFindTargetBehaviour : ITickable
    {
        public event Action Detected;
        
        private readonly Components _components;
        private readonly DetectionProps _detectionProps;

        private Transform _targetTransform;
        private Transform _currentTransform;

        public EnemyFindTargetBehaviour(Components components, DetectionProps detectionProps)
        {
            _components = components;
            _detectionProps = detectionProps;
        }

        public void Init(Transform transform, Transform playerTransform)
        {
            _components.DetectionZone.Init(_detectionProps.DetectionRadius, _detectionProps.DetectionAngle);
            _targetTransform = playerTransform;
            _currentTransform = transform;
        }
        
        public void Tick(float deltaTime)
        {
            DetectionTarget();
        }

        private void DetectionTarget()
        {
            if (_targetTransform == null)
                return;

            Vector3 distanceBetweenTarget = _targetTransform.position - _currentTransform.position;
            distanceBetweenTarget.y = 0;

            if (distanceBetweenTarget.magnitude <= _detectionProps.DetectionRadius)
            {
                float dotProduct = Vector3.Dot(distanceBetweenTarget.normalized, _currentTransform.forward);
                float detectionCos = Mathf.Cos(_detectionProps.DetectionAngle * 0.5f * Mathf.Deg2Rad);
                
                if (dotProduct > detectionCos)
                {
                    Detected?.Invoke();
                }
            }
        }
    }
}