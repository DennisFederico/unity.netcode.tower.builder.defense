using System;
using scriptables;
using Unity.Netcode;
using UnityEngine;

namespace managers {
    public class ResourceManager : NetworkBehaviour {
        //public singleton
        public static ResourceManager Instance { get; private set; }
        public event Action<ResourceTypeSO, int> OnResourceAmountChanged;
        [SerializeField] private ResourceTypeListSO resourceTypeList;
        private NetworkList<ResourceAmount> _resourceAmountList;

        public struct ResourceAmount : INetworkSerializable, IEquatable<ResourceAmount> {
            public int ResourceIndex;
            public int Amount;

            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
                serializer.SerializeValue(ref ResourceIndex);
                serializer.SerializeValue(ref Amount);
            }

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
            _resourceAmountList = new NetworkList<ResourceAmount>();
            _resourceAmountList.OnListChanged += HandleResourceAmountListChanged;
        }

        private void HandleResourceAmountListChanged(NetworkListEvent<ResourceAmount> changeEvent) {
            if (changeEvent.Type != NetworkListEvent<ResourceAmount>.EventType.Value) return;
            var resourceType = GetIndexResourceType(changeEvent.Value.ResourceIndex);
            OnResourceAmountChanged?.Invoke(resourceType, changeEvent.Value.Amount);
        }

        public override void OnNetworkSpawn() {
            base.OnNetworkSpawn();
            if (!IsServer) return;
            for (var i = 0; i < resourceTypeList.resourceTypeList.Count; i++) {
                _resourceAmountList.Add(new ResourceAmount { ResourceIndex = i, Amount = 0 });
            }
        }

        public int GetResourceTypeIndex(ResourceTypeSO resourceType) {
            return resourceTypeList.resourceTypeList.IndexOf(resourceType);
        }

        public ResourceTypeSO GetIndexResourceType(int index) {
            return resourceTypeList.resourceTypeList[index];
        }

        private void AddResource(int index, int amount) {
            var resourceAmount = _resourceAmountList[index];
            resourceAmount.Amount += amount;
            _resourceAmountList[index] = resourceAmount;
        }

        public void AddResource(ResourceTypeSO resourceType, int amount) {
            var index = GetResourceTypeIndex(resourceType);
            var resourceAmount = _resourceAmountList[index];
            resourceAmount.Amount += amount;
            _resourceAmountList[index] = resourceAmount;
        }

        public int GetResourceAmount(ResourceTypeSO resourceType) {
            var index = GetResourceTypeIndex(resourceType);
            return _resourceAmountList[index].Amount;
        }
    }
}