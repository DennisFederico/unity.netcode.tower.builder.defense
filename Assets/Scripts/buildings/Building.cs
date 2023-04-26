using System;
using managers;
using scriptables;
using Unity.Netcode;
using UnityEngine;

namespace buildings {
    [RequireComponent(typeof(BuildingTypeHolder))]
    [RequireComponent(typeof(HealthSystem))]
    public class Building : NetworkBehaviour {
        private HealthSystem _healthSystem;
        private BuildingTypeSO _buildingType;
        public event Action OnMouseHoverEnter;
        public event Action OnMouseHoverExit;

        private void Awake() {
            _buildingType = GetComponent<BuildingTypeHolder>().BuildingType;
            _healthSystem = GetComponent<HealthSystem>();
            _healthSystem.Initialize(_buildingType.maxHealth);
            _healthSystem.OnDie += HandleDie;
        }
        
        private void HandleDie() {
            DestroyServerRpc();    
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void DestroyServerRpc() {
            SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDestroyed);
            Destroy(gameObject);
        }

        private void OnMouseEnter() => OnMouseHoverEnter?.Invoke();
        
        private void OnMouseExit() => OnMouseHoverExit?.Invoke();

        public override void OnDestroy() {
            _healthSystem.OnDie -= HandleDie;
            base.OnDestroy();
        }
    }
}