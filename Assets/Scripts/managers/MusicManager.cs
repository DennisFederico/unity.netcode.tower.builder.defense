using UnityEngine;

namespace managers {
    public class MusicManager : MonoBehaviour {
        public static MusicManager Instance { get; private set; }

        public float Volume {
            get => _volume;
            set {
                _volume = Mathf.Clamp01(value);
                PlayerPrefs.SetFloat("musicVolume", _volume);
                _audioSource.volume = _volume;
            }
        }

        private float _volume = .5f;
        private AudioSource _audioSource;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }

            _volume = PlayerPrefs.GetFloat("musicVolume", .5f);
            _audioSource = GetComponent<AudioSource>();
            _audioSource.volume = _volume;
        }
    }
}