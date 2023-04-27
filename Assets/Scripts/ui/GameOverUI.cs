using managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public class GameOverUI : MonoBehaviour {
        public static GameOverUI Instance { get; private set; }
        [SerializeField] private GameObject gameOverUIContainer;
        [SerializeField] private TextMeshProUGUI wavesSurvivedText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }

            restartButton.onClick.AddListener(GameSceneManager.LoadGameScene);
            mainMenuButton.onClick.AddListener(GameSceneManager.LoadMainMenuScene);
            Hide();
        }

        public void Show() {
            wavesSurvivedText.text = $"You survived {EnemySpawner.Instance.GetCurrentHordeCount() - 1} enemy waves!!!";
            Time.timeScale = 0;
            gameOverUIContainer.SetActive(true);
        }

        private void Hide() {
            Time.timeScale = 1;
            gameOverUIContainer.SetActive(false);
        }
    }
}