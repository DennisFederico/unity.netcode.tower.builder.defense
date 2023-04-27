using System;
using System.Collections.Generic;
using resource;
using scriptables;
using UnityEngine;

namespace managers {
    // public class ResourceManager : NetworkBehaviour {
    public class ResourceManager : MonoBehaviour {
        //public singleton
        public static ResourceManager Instance { get; private set; }
        public event Action<ResourceTypeSO, int> OnResourceAmountChanged;
        [SerializeField] private ResourceTypeListSO resourceTypeList;
        //TODO reference by ResourceType not plain index
        [SerializeField] private List<int> initialResources;
        // private NetworkList<ResourceAmount> _resourceAmountList;
        private List<ResourceAmount> _resourceAmountList;

        [Serializable]
        public struct ResourceQty {
            public ResourceType ResourceType;
            public int Amount;
        }

        public struct ResourceAmount : IEquatable<ResourceAmount> {
        // public struct ResourceAmount : INetworkSerializable, IEquatable<ResourceAmount> {
            public int ResourceIndex;
            public int Amount;

            // public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            //     serializer.SerializeValue(ref ResourceIndex);
            //     serializer.SerializeValue(ref Amount);
            // }

            public bool Equals(ResourceAmount other) {
                return ResourceIndex == other.ResourceIndex && Amount == other.Amount;
            }
        }

        private void Awake() {
            //Singleton
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }

            _resourceAmountList = new ();
            // _resourceAmountList = new NetworkList<ResourceAmount>();
            // _resourceAmountList.OnListChanged += HandleResourceAmountListChanged;
        }

        private void HandleResourceAmountListChanged(ResourceAmount resourceAmount) {
            OnResourceAmountChanged?.Invoke(GetIndexResourceType(resourceAmount.ResourceIndex), resourceAmount.Amount);
        }
        // private void HandleResourceAmountListChanged(NetworkListEvent<ResourceAmount> changeEvent) {
        //     if (changeEvent.Type is not (NetworkListEvent<ResourceAmount>.EventType.Value or NetworkListEvent<ResourceAmount>.EventType.Add)) return;
        //     var resourceType = GetIndexResourceType(changeEvent.Value.ResourceIndex);
        //     OnResourceAmountChanged?.Invoke(resourceType, changeEvent.Value.Amount);
        // }

        
        
        // public override void OnNetworkSpawn() {
        public void OnEnable() {
            // base.OnNetworkSpawn();
            // if (!IsServer) return;
            for (var i = 0; i < resourceTypeList.resourceTypeList.Count; i++) {
                _resourceAmountList.Add(new ResourceAmount { ResourceIndex = i, Amount = initialResources[i] });
                HandleResourceAmountListChanged(new ResourceAmount { ResourceIndex = i, Amount = initialResources[i] });
            }
        }

        public List<ResourceTypeSO> GetResourceTypes() {
            return resourceTypeList.resourceTypeList;
        }
        
        public int GetResourceTypeIndex(ResourceTypeSO resourceType) {
            return resourceTypeList.resourceTypeList.IndexOf(resourceType);
        }

        public ResourceTypeSO GetIndexResourceType(int index) {
            return resourceTypeList.resourceTypeList[index];
        }

        public void AddResource(ResourceTypeSO resourceType, int amount) {
            AddResourceAmount(resourceType, amount);
        }

        public int GetResourceAmount(ResourceTypeSO resourceType) {
            var index = GetResourceTypeIndex(resourceType);
            return _resourceAmountList[index].Amount;
        }

        public bool CanAffordResources(ResourceCost[] resources) {
            foreach (var resource in resources) {
                var resourceAmount = GetResourceAmount(resource.ResourceTypeSO);
                if (resourceAmount < resource.Amount) return false;
            }

            return true;
        }
        
        public bool TrySpendResources(ResourceCost[] resources) {
            if (!CanAffordResources(resources)) return false;
            foreach (var resource in resources) {
                AddResourceAmount(resource.ResourceTypeSO, -resource.Amount);
            }

            return true;
        }
        
        private void AddResourceAmount(ResourceTypeSO resourceType, int amount) {
            var index = GetResourceTypeIndex(resourceType);
            var resourceAmount = _resourceAmountList[index];
            resourceAmount.Amount += amount;
            _resourceAmountList[index] = resourceAmount;
            HandleResourceAmountListChanged(resourceAmount);
        }

        public void RecoverResourcesFromDemolishBuilding(BuildingTypeSO buildingType) {
            ResourceCost[] resources = buildingType.resourceCost;
            foreach (var resource in resources) {
                AddResource(resource.ResourceTypeSO, Mathf.RoundToInt(resource.Amount*.5f));
            }
        }
    }
}