using UnityEngine;
using Game.Code.Infrastructure.Services.Factory;
using Game.Code.Infrastructure.Services.Managers;

namespace Game.Code.Logic.Bombs
{
    public class BombSpawner
    {
        private readonly IGameFactoryService _gameFactory;
        private readonly IFxManagerService _fxManagerService;

        public BombSpawner(IGameFactoryService gameFactory, IFxManagerService fxManagerService)
        {
            _gameFactory = gameFactory;
            _fxManagerService = fxManagerService;
        }

        public Bomb SpawnBomb(Transform spawnPoint)
        {
            Bomb spawnedBomb = _gameFactory.CreateBomb(spawnPoint);
            
            spawnedBomb.Init();
            spawnedBomb.Explosed += OnBombExplosed;

            return spawnedBomb;
        }

        private void OnBombExplosed(Bomb bomb, GameObject fxTemplate, Vector3 explosionPosition)
        {
            bomb.Explosed -= OnBombExplosed;
            
            _fxManagerService.PlayFX(fxTemplate, explosionPosition);
        }
    }
}