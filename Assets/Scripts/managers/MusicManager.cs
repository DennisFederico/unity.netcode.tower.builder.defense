using UnityEngine;

namespace managers {
    public class MusicManager : MonoBehaviour {
        public static MusicManager Instance { get; private set; }

        public float Volume {
            get => _audioSource.volume;
            set => _audioSource.volume = Mathf.Clamp01(value);
        }

        private AudioSource _audioSource;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }

            _audioSource = GetComponent <AudioSource>();
        }
    }
}