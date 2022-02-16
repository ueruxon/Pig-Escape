using UnityEngine;
using Game.Code.Infrastructure.Services;
using Game.Code.Infrastructure.Services.Managers;
using Game.Code.Logic.Players;

namespace Game.Code.Logic
{
    public class FinishZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                var gameManager = ServiceLocator.Container.GetSingle<IGameManagerService>();
                gameManager.StopGame(true);
            }
        }
    }
}