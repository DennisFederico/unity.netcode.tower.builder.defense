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
            Destroy(gameObject);
        }

        public override void OnDestroy() {
            _healthSystem.OnDie -= HandleDie;
            base.OnDestroy();
        }
    }
}