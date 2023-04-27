using managers;
using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public class MainMenuUI : MonoBehaviour {
        [SerializeField] private Button playButton;
        [SerializeField] private Button exitButton;

        private void Awake() {
            playButton.onClick.AddListener(OnPlayButtonClicked);
            exitButton.onClick.AddListener(Application.Quit);
        }

        private void OnPlayButtonClicked() {
            GameSceneManager.LoadGameScene();
        }
    }
}