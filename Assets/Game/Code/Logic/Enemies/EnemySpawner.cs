using System.Collections.Generic;
using Game.Code.Common;
using Game.Code.Infrastructure.Services.Factory;
using Game.Code.Logic.Players;
using UnityEngine;

namespace Game.Code.Logic.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy _enemyTemplate;
        [SerializeField] private List<Transform> _waypointList;
        
        private IGameFactoryService _gameFactory;
        private IDamageble _player;
        private Enemy _currentEnemy;

        // Это костыль конечно, передавать игрока врагу. Но че поделать.
        public void Init(IGameFactoryService gameFactory, IDamageble player)
        {
            _gameFactory = gameFactory;
            _player = player;
        }

        public void Spawn()
        {
            _currentEnemy = _gameFactory.CreateEnemy(_enemyTemplate, transform.position);
            _currentEnemy.Init(_waypointList, _player);
        }

        public void Stop() => 
            _currentEnemy.Disable();
    }
}