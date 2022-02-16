using Game.Code.Infrastructure.Services.Managers;
using Game.Code.UI.Elements;
using UnityEngine;

namespace Game.Code.UI
{
    public class HudUI : MonoBehaviour
    {
        [SerializeField] private ResultPanelUI _resultPanel;
        
        public void Init(IGameManagerService gameManager)
        {
            _resultPanel.Init(gameManager);
            _resultPanel.Hide();
        }
    }
}