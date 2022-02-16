using UnityEngine;
using Game.Code.Infrastructure.Services;
using Game.Code.Infrastructure.Services.AssetManagement;
using Game.Code.Infrastructure.Services.Factory;
using Game.Code.Infrastructure.Services.Inputs;
using Game.Code.Infrastructure.Services.Managers;
using Game.Code.Logic.Bombs;
using Game.Code.Logic.CameraLogic;
using Game.Code.Logic.Enemies;
using Game.Code.Logic.Players;
using Game.Code.UI;

namespace Game.Code.Core
{
    public class GameInstaller
    {
        private const string InitialPoint = "InitialPoint";

        private IAssetProviderService _assetProvider;
        private IGameFactoryService _gameFactory;
        private IInputService _inputService;
        private IGameManagerService _gameManager;
        private IFxManagerService _fxManager;

        private GameLoop _gameLoop;
        
        public GameInstaller()
        {
            if (Application.isEditor)
            {
                Application.targetFrameRate = 70;
            }
            
            RegisterServices();
        }

        private void RegisterServices()
        {
            _assetProvider = new AssetProvider();
            _gameFactory = new GameFactory(_assetProvider);
            _inputService = InputService();
            _gameManager = new GameManager();
            _fxManager = new FxManager(_gameFactory);

            ServiceLocator.Container.Register(_assetProvider);
            ServiceLocator.Container.Register(_gameFactory);
            ServiceLocator.Container.Register(_inputService);
            ServiceLocator.Container.Register(_gameManager);
            ServiceLocator.Container.Register(_fxManager);
        }
        
        private IInputService InputService() {
            if (Application.isEditor)
                return new StandaloneInput();
            else
                return new MobileInput();
        }
        
        public void InitLevel()
        {
            BombSpawner bombSpawner = InitBombSpawner();
            Player player = InitPlayer(bombSpawner);
            InitHud();
            EnemySpawner[] spawners = InitEnemySpawners(player);

            _gameLoop = new GameLoop(_gameManager, _inputService, player, spawners);
            _gameLoop.Start();
        }

        private BombSpawner InitBombSpawner() => 
            new BombSpawner(_gameFactory, _fxManager);

        private Player InitPlayer(BombSpawner bombSpawner)
        {
            var initialPoint = GameObject.FindWithTag(InitialPoint);
            Player player = _gameFactory.CreatePlayer(initialPoint.transform);

            player.Init(_inputService, bombSpawner);
            CameraFollow(player.gameObject);

            return player;
        }

        private void InitHud()
        {
            HudUI hudUI = _gameFactory.CreateHud().GetComponent<HudUI>();
            hudUI.Init(_gameManager);
        }

        private EnemySpawner[] InitEnemySpawners(Player player)
        {
            EnemySpawner[] enemySpawners = Object.FindObjectsOfType<EnemySpawner>();

            foreach (EnemySpawner enemySpawner in enemySpawners) 
                enemySpawner.Init(_gameFactory, player);

            return enemySpawners;
        }

        private void CameraFollow(GameObject target) =>
            Camera.main
                .GetComponent<CameraFollow>()
                .Follow(target);
    }
}