using System;
using scriptables;
using Unity.Netcode;
using UnityEngine;

namespace managers {
    public class ResourceManager : NetworkBehaviour {
        //public singleton
        public static ResourceManager Instance { get; private set; }
        public event Action<ResourceTypeSO, int> OnResourceAmountChanged;
        private ResourceTypeListSO _resourceTypeList;
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

        public enum ResourceType {
            Wood,
            Stone,
            Gold
        }

        private void Awake() {
            //Singleton
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }

            _resourceTypeList = Resources.Load<ResourceTypeListSO>(nameof(ResourceTypeListSO));
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
            for (var i = 0; i < _resourceTypeList.resourceTypeList.Count; i++) {
                _resourceAmountList.Add(new ResourceAmount { ResourceIndex = i, Amount = 0 });
            }
        }

        private void Update() {
            if (!IsServer) return;

            if (Input.GetKeyDown(KeyCode.Q)) {
                AddResource(0, 15);
            }

            if (Input.GetKeyDown(KeyCode.W)) {
                AddResource(_resourceTypeList.resourceTypeList[1], 10);
            }

            if (Input.GetKeyDown(KeyCode.E)) {
                AddResource(2, 5);
            }
        }

        public int GetResourceTypeIndex(ResourceTypeSO resourceType) {
            return _resourceTypeList.resourceTypeList.IndexOf(resourceType);
        }

        public ResourceTypeSO GetIndexResourceType(int index) {
            return _resourceTypeList.resourceTypeList[index];
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