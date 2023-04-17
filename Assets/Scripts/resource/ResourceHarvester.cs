using System.Diagnostics.CodeAnalysis;
using managers;
using scriptables;
using Unity.Netcode;
using UnityEngine;

namespace resource {
    public class ResourceHarvester : NetworkBehaviour {
        [SerializeField] private BuildingTypeSO buildingType;
        private float _timerMax;
        private float _timer;
        private Collider2D[] _resourcesInRange;
        private int _currentHarvestAmount;
        private ResourceHarvestData _resourceHarvestData;

        private void Awake() {
            _timerMax = buildingType.resourceHarvestData.timerMax;
            _resourceHarvestData = buildingType.resourceHarvestData;
        }

        public override void OnNetworkSpawn() {
            base.OnNetworkSpawn();
            //TODO this should be done on the server
            //TODO this should be called again whenever a resource node exhausts
            _currentHarvestAmount = GetResourcesInRange(out _resourcesInRange);
            _timerMax = CalculateTimerMax();
            
            //Disable if no resources in range
            if (_currentHarvestAmount == 0) {
                enabled = false;
            }
            Debug.Log($"Spawned Resource Harvesting {_resourcesInRange.Length} {_resourceHarvestData.resourceType.resourceName} in range : enabled? {enabled}");
        }

        private void Update() {
            if (!IsServer) return;
            _timer -= Time.deltaTime;

            if (_timer > 0) return;
            _timer += _timerMax;

            //TODO Check when a node exhausts

            ResourceManager.Instance.AddResource(_resourceHarvestData.resourceType, _currentHarvestAmount);
        }

        [SuppressMessage("ReSharper", "Unity.PreferNonAllocApi")]
        private int GetResourcesInRange(out Collider2D[] resourcesInRange) {
            //TODO Clamp using NonAllocApi using _resourceHarvestData.maxHarvestNodes
            resourcesInRange = Physics2D.OverlapCircleAll(transform.position, _resourceHarvestData.harvestRadius, _resourceHarvestData.resourceLayer);
            return resourcesInRange.Length;
        }

        private float CalculateTimerMax() {
            return (_resourceHarvestData.timerMax) + (_resourceHarvestData.timerMax * (1 - (float)_currentHarvestAmount / _resourceHarvestData.maxHarvestNodes));
        }
        
        public ResourceTypeSO GetResourceType() {
            return _resourceHarvestData.resourceType;
        }
        
        public float GetNormalizedHarvestTimer() {
            return _timer / _timerMax;
        }

        public float GetHarvestAmountPerSecond() {
            return _currentHarvestAmount / _timerMax;
        }
    }
}