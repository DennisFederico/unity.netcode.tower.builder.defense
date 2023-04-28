using System;
using managers;
using scriptables;
using UnityEngine;

namespace buildings {
    [RequireComponent(typeof(BuildingTypeHolder))]
    [RequireComponent(typeof(HealthSystem))]
    // public class Building : NetworkBehaviour {
    public class Building : MonoBehaviour {
        private HealthSystem _healthSystem;
        [SerializeField] private GameObject destroyEffect;
        private BuildingTypeSO _buildingType;
        public event Action OnMouseHoverEnter;
        public event Action OnMouseHoverExit;

        private void Awake() {
            _buildingType = GetComponent<BuildingTypeHolder>().BuildingType;
            _healthSystem = GetComponent<HealthSystem>();
            _healthSystem.Initialize(_buildingType.maxHealth);
            _healthSystem.OnDamage += HandleOnDamage;
            _healthSystem.OnDie += HandleOnDie;
        }

        private void HandleOnDamage() {
            CineMachineShake.Instance.ShakeCamera(7f, .15f);
            ChromaticAberrationEffect.Instance.SetWeight(.5f);
        }

        private void HandleOnDie() {
            CineMachineShake.Instance.ShakeCamera(10f, .2f);
            ChromaticAberrationEffect.Instance.SetWeight(1f);
            DestroyServerRpc();
        }
        
        // [ServerRpc(RequireOwnership = false)]
        private void DestroyServerRpc() {
            SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDestroyed);
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        private void OnMouseEnter() => OnMouseHoverEnter?.Invoke();
        
        private void OnMouseExit() => OnMouseHoverExit?.Invoke();

        // public override void OnDestroy() {
        public void OnDestroy() {
            _healthSystem.OnDamage -= HandleOnDamage;
            _healthSystem.OnDie -= HandleOnDie;
            // base.OnDestroy();
        }
    }
}