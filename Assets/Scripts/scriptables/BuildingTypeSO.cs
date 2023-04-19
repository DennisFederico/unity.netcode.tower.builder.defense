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
        public ResourceCost[] resourceCost;
        
        public string GetBuildingCostString() {
            var costString = "";
            foreach (var rCost in resourceCost) {
                costString += $"<color=#{ColorUtility.ToHtmlStringRGB(rCost.ResourceTypeSO.color)}>" +
                              $"{rCost.ResourceTypeSO.shortName}{rCost.Amount}" + 
                              "</color> ";
            }
            return costString;
        }
    }
}