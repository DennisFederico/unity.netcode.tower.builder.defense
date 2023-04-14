using scriptables;
using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public class BuildingSelectUIBtn : MonoBehaviour {

        [SerializeField] private Image icon;
        [SerializeField] private Image selected;
        public void Initialize(BuildingTypeSO buildingType) {
            icon.sprite = buildingType.sprite;
        }
        
        public void SetSelected(bool isSelected) {
            selected.gameObject.SetActive(isSelected);
        }
    }
}
