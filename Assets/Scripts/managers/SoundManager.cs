using scriptables;
using UnityEngine;

namespace managers {
    public class SoundManager : MonoBehaviour {

        public enum Sound {
            BuildingDamaged,
            BuildingDestroyed,
            BuildingPlaced,
            EnemyDie,
            EnemyHit,
            EnemyWaveStarting,
            GameOver,
            Music,
        }
        
        public static SoundManager Instance { get; private set; }
        
        
        public float Volume {
            get => _volume;
            set => _volume = Mathf.Clamp01(value);
        }

        [SerializeField] private SoundFxMap audioClipsMap;
        private AudioSource _audioSource;
        private float _volume = .5f;
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }

            _audioSource = GetComponent<AudioSource>();
        }
        
        public void PlaySound(Sound sound) {
            _audioSource.PlayOneShot(audioClipsMap.audioClips[sound], Volume);
        }
    }
}