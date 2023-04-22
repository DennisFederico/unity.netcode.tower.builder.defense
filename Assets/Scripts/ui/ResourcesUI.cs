using System.Collections.Generic;
using managers;
using scriptables;
using UnityEngine;

namespace ui {
    public class ResourcesUI : MonoBehaviour {
        [SerializeField] private Transform resourceSinglePrefab;
        private Dictionary<ResourceTypeSO, ResourceUISingle> _resourceUISingleDictionary = new();

        private void Start() {
            foreach (var resourceType in ResourceManager.Instance.GetResourceTypes()) {
                var resourceSingle = Instantiate(resourceSinglePrefab, transform);
                var resourceUISingle = resourceSingle.GetComponent<ResourceUISingle>();
                resourceUISingle.Initialize(resourceType);
                _resourceUISingleDictionary[resourceType] = resourceUISingle;
                HandleResourceAmountChanged(resourceType, ResourceManager.Instance.GetResourceAmount(resourceType));
            }
            ResourceManager.Instance.OnResourceAmountChanged += HandleResourceAmountChanged;
        }

        private void HandleResourceAmountChanged(ResourceTypeSO resourceTypeSO, int amount) {
            var resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceTypeSO);
            if (_resourceUISingleDictionary.TryGetValue(resourceTypeSO, out var resourceUISingle)) {
                resourceUISingle.UpdateAmount(resourceAmount);
            }
        }
    }
}