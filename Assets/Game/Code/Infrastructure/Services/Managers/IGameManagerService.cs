using System;

namespace Game.Code.Infrastructure.Services.Managers
{
    public interface IGameManagerService : IService
    {
        public event Action<GameState> GameStateUpdated; 
        void StartGame();
        void StopGame(bool gameSuccess);
        void RestartGame();
        void ExitGame();
    }
}