using UnityEngine;
using Game.Code.Infrastructure.Services.AssetManagement;
using Game.Code.Logic.Bombs;
using Game.Code.Logic.Enemies;
using Game.Code.Logic.Players;

namespace Game.Code.Infrastructure.Services.Factory
{
    public class GameFactory : IGameFactoryService
    {
        private readonly IAssetProviderService _assetProvider;

        public GameFactory(IAssetProviderService assetProvider) => 
            _assetProvider = assetProvider;
        
        public Player CreatePlayer(Transform at)
        {
            var template = _assetProvider.GetGameObject(AssetPath.PlayerPath);
            return Object.Instantiate(template, at.position, template.transform.rotation)
                .GetComponent<Player>();
        }

        public GameObject CreateHud()
        {
            var template = _assetProvider.GetGameObject(AssetPath.HudPath);
            return Object.Instantiate(template);
        }

        public Enemy CreateEnemy(Enemy enemyTemplate, Vector3 transformPosition) => 
            Object.Instantiate(enemyTemplate, transformPosition, Quaternion.identity);

        public Bomb CreateBomb(Transform at)
        {
            var template = _assetProvider.GetGameObject(AssetPath.BombPath);
            return Object.Instantiate(template, at)
                .GetComponent<Bomb>();
        }

        public GameObject CreateFx(GameObject fxTemplate, Vector3 at) => 
            Object.Instantiate(fxTemplate, at, Quaternion.identity);
    }
}