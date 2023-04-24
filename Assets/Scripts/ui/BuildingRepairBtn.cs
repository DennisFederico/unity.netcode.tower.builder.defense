using buildings;
using managers;
using resource;
using scriptables;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ui {
    public class BuildingRepairBtn : MonoBehaviour {

        [SerializeField] private Button button;
        [SerializeField] private HealthSystem healthSystem;
        [SerializeField] private ResourceTypeSO repairResourceType;
        private int _repairResourceBaseAmount = 50;
        private BuildingTypeSO _buildingType;
        
        private void Awake() {
            _buildingType = healthSystem.GetComponent<BuildingTypeHolder>().BuildingType;
            button.onClick.AddListener(() => {
                var resourceCosts = new[] { new ResourceCost(repairResourceType, Mathf.RoundToInt(healthSystem.CurrentHealthNormalized * _repairResourceBaseAmount)) };
                if (ResourceManager.Instance.TrySpendResources(resourceCosts)) {
                    healthSystem.HealFull();
                } else {
                    TooltipUI.Instance.Show("Cannot afford repair", 1.5f);
                }
            });
            healthSystem.OnDamage += () => {
                button.gameObject.SetActive(true);
            };
            healthSystem.OnHeal += () => {
                button.gameObject.SetActive(!healthSystem.IsFullHealth);
            };
        }

        private void Start() {
            button.gameObject.SetActive(false);
        }
    }
}