using System;
using buildings;
using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public class ConstructionTimerUI : MonoBehaviour {
        
        [SerializeField] private BuildingConstruction buildingConstruction;
        [SerializeField] private Image timerImage;

        private void Update() {
            timerImage.fillAmount = buildingConstruction.ConstructionTimerNormalized;
        }
    }
}