using System.Collections.Generic;
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
        [SerializeField] private SoundFxMap audioClipsMap;
        private AudioSource _audioSource;
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }

            _audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(Sound sound) {
            _audioSource.PlayOneShot(audioClipsMap.audioClips[sound]);
        }
    }
}