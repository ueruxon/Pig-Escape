using Game.Code.Infrastructure.Services.Inputs;
using Game.Code.Infrastructure.Services.Managers;
using Game.Code.Logic.Enemies;
using Game.Code.Logic.Players;
using UnityEngine;

namespace Game.Code.Core
{
    public class GameLoop
    {
        private readonly IGameManagerService _gameManager;
        private readonly IInputService _inputService;
        private readonly Player _player;
        private readonly EnemySpawner[] _enemySpawners;

        public GameLoop(IGameManagerService gameManager, IInputService inputService, Player player, EnemySpawner[] enemySpawners)
        {
            _gameManager = gameManager;
            _inputService = inputService;
            _player = player;
            _enemySpawners = enemySpawners;
            
            _gameManager.GameStateUpdated += OnGameStateUpdated;
        }

        public void Start()
        {
            _gameManager.StartGame();

            SpawnEnemies();
            _player.Failured += OnPlayerFailured;
        }

        private void OnGameStateUpdated(GameState gameState)
        {
            if (gameState != GameState.GameLoop)
            {
                DisableEnemies();
                _player.Disable();
            }
        }

        private void OnPlayerFailured()
        {
            DisableEnemies();
            
            _player.Disable();
            _gameManager.StopGame(false);
        }

        private void SpawnEnemies()
        {
            foreach (EnemySpawner enemySpawner in _enemySpawners) 
                enemySpawner.Spawn();
        }

        private void DisableEnemies()
        {
            foreach (EnemySpawner enemySpawner in _enemySpawners) 
                enemySpawner.Stop();
        }
    }
}