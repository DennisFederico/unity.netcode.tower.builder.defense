using managers;
using scriptables;
using Unity.Netcode;
using UnityEngine;

namespace resource {
    public class ResourceGenerator : NetworkBehaviour {
        [SerializeField] private BuildingTypeSO buildingType;
        private float _timerMax;
        private float _timer;

        private void Awake() {
            _timerMax = buildingType.resourceGenerationData.timer;
        }

        private void Update() {
            if (!IsServer) return;
            _timer -= Time.deltaTime;
            if(_timer > 0) return;
            _timer += _timerMax;
            ResourceManager.Instance.AddResource(buildingType.resourceGenerationData.resourceType, 1);
        }
    }
}