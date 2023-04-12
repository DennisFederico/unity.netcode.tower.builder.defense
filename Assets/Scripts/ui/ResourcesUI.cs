using managers;
using scriptables;
using TMPro;
using UnityEngine;

namespace ui {
    public class ResourcesUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI woodText;
        [SerializeField] private TextMeshProUGUI stoneText;
        [SerializeField] private TextMeshProUGUI goldText;
        
        private void Start() {
            ResourceManager.Instance.OnResourceAmountChanged += HandleResourceAmountChanged;
        }

        private void HandleResourceAmountChanged(ResourceTypeSO resourceTypeSO, int amount) {
            var resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceTypeSO);
            switch (resourceTypeSO.resourceType) {
                case ResourceManager.ResourceType.Wood:
                    woodText.text = resourceAmount.ToString();
                    break;
                case ResourceManager.ResourceType.Stone:
                    stoneText.text = resourceAmount.ToString();
                    break;
                case ResourceManager.ResourceType.Gold:
                    goldText.text = resourceAmount.ToString();
                    break;
            }
        }
    }
}