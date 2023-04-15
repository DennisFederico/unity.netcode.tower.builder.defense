using System;
using scriptables;
using UnityEngine;

namespace resource {
    
    [Serializable]
    public struct ResourceHarvestData {
        public float timerMax;
        public ResourceTypeSO resourceType;
        public LayerMask resourceLayer;
        public float harvestRadius;
        public int maxHarvestNodes;
    }
}