using System;
using UnityEngine;
using DG.Tweening;
using Game.Code.Common;

namespace Game.Code.Logic.Bombs
{
    public class Bomb : MonoBehaviour
    {
        public event Action<Bomb, GameObject, Vector3> Explosed;
        
        [SerializeField] private Components _components;
        [SerializeField] private float _explosionRadius;
        
        private Sequence _jumpSequence;

        public void Init()
        {
            _components._collider.radius = _explosionRadius;
            _components._collider.enabled = false;
        }

        public void MoveTo(Vector3 targetPosition)
        {
            transform.SetParent(null);
            
            _jumpSequence = transform.DOLocalJump(
                endValue: targetPosition,
                jumpPower: 1,
                numJumps: 1,
                duration: 1f
            )
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _components._collider.enabled = true;
                    _components.RadiusIndicator.Init(_explosionRadius);
                });
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageble damageble)) 
                Detonation(damageble);
        }

        private void Detonation(IDamageble damageble)
        {
            damageble.ApplyDamage();
            Explosed?.Invoke(this, _components.ExplosionFx, transform.position);
            
            _jumpSequence.Kill();
            Destroy(gameObject);
        }
    }

    [System.Serializable]
    public struct Components
    {
        public RadiusIndicator RadiusIndicator;
        public SphereCollider _collider;
        public TrailRenderer _trail;
        public GameObject ExplosionFx;
    }
}