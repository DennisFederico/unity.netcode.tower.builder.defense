using scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public class ResourceUISingle : MonoBehaviour {

        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI text;

        public void Initialize(ResourceTypeSO resourceType) {
            icon.sprite = resourceType.iconSprite;
            text.text = "0";
        }

        public void UpdateAmount(int resourceAmount) {
            text.text = resourceAmount.ToString();
        }
    }
}