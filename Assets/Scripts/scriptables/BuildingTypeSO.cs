using resource;
using UnityEngine;

namespace scriptables {
    [CreateAssetMenu (menuName = "ScriptableObjects/BuildingType")]
    public class BuildingTypeSO : ScriptableObject {
        public string buildingName;
        public Transform buildingPrefab;
        public Sprite sprite;
        public ResourceHarvestData resourceHarvestData;
        public Vector2 safeFromBuildingsDistance;
        public float maxDistanceFromBuildings;
    }
}