using UnityEngine;

namespace scriptables {
    [CreateAssetMenu (menuName = "ScriptableObjects/BuildingType")]
    public class BuildingTypeSO : ScriptableObject {
        public string buildingName;
        public Transform buildingPrefab;
        public ResourceGenerationData resourceGenerationData;
    }
}