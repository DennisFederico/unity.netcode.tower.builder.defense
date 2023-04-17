using resource;
using TMPro;
using UnityEngine;
using utils;

namespace ui {
    public class ResourceNearbyOverlay : MonoBehaviour {
        [SerializeField] private SpriteRenderer harvestResourceIcon;
        [SerializeField] private TextMeshPro nodesInRangeText;
        [SerializeField] private TextMeshPro efficiencyText;
        
        public void Show(ResourceHarvestData resourceHarvestData) {
            harvestResourceIcon.sprite = resourceHarvestData.resourceType.sprite;
            UpdateText(resourceHarvestData);
            gameObject.SetActive(true);
        }
        
        public void Hide() {
            gameObject.SetActive(false);
        }
        
        public void UpdateText(ResourceHarvestData resourceHarvestData) {
            var worldMousePosition = CursorManager.Instance.GetWorldMousePosition();
            int nodesInRange = ResourceHarvester.GetResourceNodesInRangeCount(resourceHarvestData, worldMousePosition);
            nodesInRangeText.text = $"{nodesInRange}/{resourceHarvestData.maxHarvestNodes}";
            nodesInRange = Mathf.Clamp(nodesInRange, 0, resourceHarvestData.maxHarvestNodes);
            var efficiency = ((float)nodesInRange / resourceHarvestData.maxHarvestNodes) * 100;
            efficiencyText.text = efficiency >= 100f ? $"100%" : $"{efficiency:#.0}%";
        }
    }
}