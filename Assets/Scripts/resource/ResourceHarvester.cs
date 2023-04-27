using System.Diagnostics.CodeAnalysis;
using buildings;
using managers;
using scriptables;
using UnityEngine;

namespace resource {
    [RequireComponent(typeof(BuildingTypeHolder))]
    // public class ResourceHarvester : NetworkBehaviour {
    public class ResourceHarvester : MonoBehaviour {
        private BuildingTypeSO _buildingType;
        private float _timerMax;
        private float _timer;
        private Collider2D[] _resourcesInRange;
        private int _currentHarvestAmount;
        private ResourceHarvestData _resourceHarvestData;

        [SuppressMessage("ReSharper", "Unity.PreferNonAllocApi")]
        public static Collider2D[] GetResourceNodesInRange(ResourceHarvestData resourceHarvestData, Vector2 areaCenter) {
            return Physics2D.OverlapCircleAll(areaCenter, resourceHarvestData.harvestRadius, resourceHarvestData.resourceLayer);
        }
        
        public static int GetResourceNodesInRangeCount(ResourceHarvestData resourceHarvestData, Vector2 areaCenter) {
            return GetResourceNodesInRange(resourceHarvestData, areaCenter).Length;
        }
        
        private void Awake() {
            _buildingType = GetComponent<BuildingTypeHolder>().BuildingType;
            _timerMax = _buildingType.resourceHarvestData.timerMax;
            _resourceHarvestData = _buildingType.resourceHarvestData;
        }

        //public override void OnNetworkSpawn() {
        private void OnEnable() {
            // base.OnNetworkSpawn();
            //TODO this should be done on the server
            //TODO this should be called again whenever a resource node exhausts
            _resourcesInRange = GetResourceNodesInRange(_resourceHarvestData, transform.position);
            _currentHarvestAmount = _currentHarvestAmount = Mathf.Clamp(_resourcesInRange.Length, 0, _resourceHarvestData.maxHarvestNodes);
            _timerMax = CalculateTimerMax();
            
            //Disable if no resources in range
            if (_currentHarvestAmount == 0) {
                enabled = false;
            }
        }

        private void Update() {
            // if (!IsServer) return;
            _timer -= Time.deltaTime;

            if (_timer > 0) return;
            _timer += _timerMax;

            //TODO Check when a node exhausts

            ResourceManager.Instance.AddResource(_resourceHarvestData.resourceType, _currentHarvestAmount);
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