using System.Collections;
using UnityEngine;
using Game.Code.Common;
using Game.Code.Infrastructure.Services.Inputs;
using Game.Code.Logic.Bombs;

namespace Game.Code.Logic.Players
{
    public class PlayerCombat : ITickable
    {
        private readonly IInputService _inputService;
        private readonly BombSpawner _bombSpawner;
        private readonly CombatProps _props;
        private readonly ICoroutineRunner _coroutineRunner;

        private readonly WaitForSeconds _delay = new WaitForSeconds(0.1f);

        private Bomb _currentBomb;
        
        public PlayerCombat(IInputService inputService, BombSpawner bombSpawner, CombatProps props, ICoroutineRunner coroutineRunner)
        {
            _inputService = inputService;
            _bombSpawner = bombSpawner;
            _props = props;
            _coroutineRunner = coroutineRunner;
        }

        public void Init() => 
            _coroutineRunner.StartCoroutine(BombReloadRoutine());

        public void Tick(float deltaTime)
        {
            if (_inputService.AttackButtonUp())
            {
                TryThrowBomb();
            }
        }

        private void TryThrowBomb()
        {
            if (_currentBomb != null)
            {
                _currentBomb.MoveTo(_props.BombDropPoint.position);
                _currentBomb = null;

                _coroutineRunner.StartCoroutine(BombReloadRoutine());
            }
        }

        private IEnumerator BombReloadRoutine()
        {
            _props.ReloadIndicator.Show();
            
            float currentReloadTimer = 0;

            while (currentReloadTimer < _props.Cooldown)
            {
                yield return _delay;

                currentReloadTimer += 0.1f;
                _props.ReloadIndicator.SetIndicatorValue(currentReloadTimer / _props.Cooldown);
            }

            _currentBomb = _bombSpawner.SpawnBomb(_props.BombContainer);
            _props.ReloadIndicator.Hide();
        }
    }
}