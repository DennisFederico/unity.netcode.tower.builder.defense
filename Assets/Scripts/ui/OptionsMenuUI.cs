using managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public class OptionsMenuUI : MonoBehaviour {
        [SerializeField] private GameObject optionsMenu;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private TextMeshProUGUI sfxVolumeText;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private TextMeshProUGUI musicVolumeText;
        [SerializeField] private Button mainMenuButton;

        private void Awake() {
            optionsMenu.SetActive(false);
            mainMenuButton.onClick.AddListener(() => Debug.Log("Main Menu Button Clicked"));
        }

        private void Start() {
            sfxVolumeSlider.value = SoundManager.Instance.Volume;
            sfxVolumeText.text = Mathf.RoundToInt(SoundManager.Instance.Volume * 100).ToString();
            sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeSliderValueChanged);
            
            musicVolumeSlider.value = MusicManager.Instance.Volume;
            musicVolumeText.text = Mathf.RoundToInt(MusicManager.Instance.Volume * 100).ToString();
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeSliderValueChanged);
        }

        private void OnMusicVolumeSliderValueChanged(float volume) {
            musicVolumeText.text = Mathf.RoundToInt(volume * 100).ToString();
            MusicManager.Instance.Volume = volume;
        }

        private void OnSfxVolumeSliderValueChanged(float volume) {
            sfxVolumeText.text = Mathf.RoundToInt(volume * 100).ToString();
            SoundManager.Instance.Volume = volume;
        }

        public void Toggle() {
            optionsMenu.SetActive(!optionsMenu.activeSelf);
            if (optionsMenu.activeSelf) {
                Time.timeScale = 0;
            } else {
                Time.timeScale = 1;
            }
        }
    }
}