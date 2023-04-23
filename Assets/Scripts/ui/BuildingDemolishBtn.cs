using System;
using buildings;
using managers;
using scriptables;
using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public class BuildingDemolishBtn : MonoBehaviour {

        [SerializeField] private Button button;
        [SerializeField] private Building building;
        private BuildingTypeSO _buildingType;
        
        private void Awake() {
            _buildingType = building.GetComponent<BuildingTypeHolder>().BuildingType;
            button.onClick.AddListener(() => {
                Destroy(building.gameObject);
                ResourceManager.Instance.RecoverResourcesFromDemolishBuilding(_buildingType);
            });
            building.OnMouseHoverEnter += () => {
                button.gameObject.SetActive(true);
            };
            building.OnMouseHoverExit += () => {
                button.gameObject.SetActive(false);
            };
        }

        private void Start() {
            button.gameObject.SetActive(false);
        }
    }
}