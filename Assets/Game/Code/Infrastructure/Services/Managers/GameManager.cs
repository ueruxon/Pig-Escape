using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Code.Infrastructure.Services.Managers
{
    public enum GameState
    {
        GameLoop,
        Win,
        Lose
    }
    
    public class GameManager : IGameManagerService
    {
        public event Action<GameState> GameStateUpdated; 

        private GameState _gameState;

        public void StartGame() => 
            SetState(GameState.GameLoop);

        public void StopGame(bool gameSuccess) => 
            SetState(gameSuccess ? GameState.Win : GameState.Lose);

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GC.Collect();
        }

        public void ExitGame() => 
            Application.Quit();

        private void SetState(GameState nextState)
        {
            _gameState = nextState;
            GameStateUpdated?.Invoke(_gameState);
        }
    }
}