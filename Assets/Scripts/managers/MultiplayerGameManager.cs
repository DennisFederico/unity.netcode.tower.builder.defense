using Unity.Netcode;
using UnityEngine;

namespace managers {
    public class MultiplayerGameManager : NetworkBehaviour {
        public static MultiplayerGameManager Instance { get; private set; }
        
        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }
        }
        
        public void SendBuildingSpawnRequest(int buildingIndex, Vector3 position) {
            ServerSpawnBuildingServerRpc(buildingIndex, position);
        }
        
        [ServerRpc (RequireOwnership = false)]
        private void ServerSpawnBuildingServerRpc(int buildingIndex, Vector3 position) {
            //Get BuildingTypeSO from BuildingTypeListSO in BuildingManager
            var buildingTypes = BuildingManager.Instance.GetBuildingTypeListSO();
            var buildingPrefab = buildingTypes.GetBuildingPrefabByIndex(buildingIndex);
            var buildingInstance = Instantiate(buildingPrefab, position, Quaternion.identity);
            buildingInstance.GetComponent<NetworkObject>().Spawn(true);
        }
    }
}