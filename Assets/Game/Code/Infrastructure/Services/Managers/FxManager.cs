using Game.Code.Infrastructure.Services.Factory;
using UnityEngine;

namespace Game.Code.Infrastructure.Services.Managers
{
    public class FxManager : IFxManagerService
    {
        private readonly IGameFactoryService _gameFactory;

        public FxManager(IGameFactoryService gameFactory) => 
            _gameFactory = gameFactory;

        public void PlayFX(GameObject fxTemplate, Vector3 at) => 
            _gameFactory.CreateFx(fxTemplate, at);
    }

    public interface IFxManagerService : IService
    {
        void PlayFX(GameObject fxTemplate, Vector3 at);
    }
}