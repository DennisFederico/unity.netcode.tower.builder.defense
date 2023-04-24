using System;
using scriptables;
using UnityEngine;

namespace resource {
    
    [Serializable]
    public struct ResourceCost {
        [SerializeField] private ResourceTypeSO resourceTypeSO;
        [SerializeField] private int amount;

        public ResourceCost(ResourceTypeSO resourceTypeSO, int amount) {
            this.resourceTypeSO = resourceTypeSO;
            this.amount = amount;
        }

        public int Amount => amount;
        public ResourceTypeSO ResourceTypeSO => resourceTypeSO;
    }
}