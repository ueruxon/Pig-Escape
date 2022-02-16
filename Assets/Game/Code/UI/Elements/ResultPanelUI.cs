using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Code.Infrastructure.Services.Managers;

namespace Game.Code.UI.Elements
{
    public class ResultPanelUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _resultText;
        [SerializeField] private Button _exitButton, _restartButton;

        private IGameManagerService _gameManager;
        
        public void Init(IGameManagerService gameManager)
        {
            _gameManager = gameManager;
            _gameManager.GameStateUpdated += OnGameStateUpdated;

            _exitButton.onClick.AddListener(_gameManager.ExitGame);
            _restartButton.onClick.AddListener(_gameManager.RestartGame);
        }

        public void Show() => 
            gameObject.SetActive(true);

        public void Hide() => 
            gameObject.SetActive(false);

        private void OnGameStateUpdated(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Win:
                    _resultText.SetText("Свобода!!!");
                    Show();
                    break;
                case GameState.Lose:
                    _resultText.SetText("Провал...");
                    Show();
                    break;
            }
        }
    }
}