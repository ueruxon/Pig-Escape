using Game.Code.Logic.Bombs;
using Game.Code.Logic.Enemies;
using UnityEngine;
using Game.Code.Logic.Players;

namespace Game.Code.Infrastructure.Services.Factory
{
    public interface IGameFactoryService : IService
    {
        Player CreatePlayer(Transform at);
        GameObject CreateHud();
        Bomb CreateBomb(Transform at);
        Enemy CreateEnemy(Enemy enemyTemplate, Vector3 transformPosition);
        GameObject CreateFx(GameObject fxTemplate, Vector3 at);
    }
}