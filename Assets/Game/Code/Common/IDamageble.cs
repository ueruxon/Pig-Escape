using UnityEngine;

namespace Game.Code.Common
{
    public interface IDamageble
    {
        void ApplyDamage();
        Transform GetTransform();
    }
}