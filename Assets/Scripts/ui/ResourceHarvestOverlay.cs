using resource;
using TMPro;
using UnityEngine;

namespace ui {
    public class ResourceHarvestOverlay : MonoBehaviour {
        [SerializeField] private ResourceHarvester resourceHarvester;
        [SerializeField] private SpriteRenderer harvestResourceIcon;
        [SerializeField] private Transform harvestProgressBar;
        [SerializeField] private TextMeshPro harvestSpeedText;
        private float _textUpdateTimer = 3f;
        private float _timer;

        private void Start() {
            harvestResourceIcon.sprite = resourceHarvester.GetResourceType().sprite;
            harvestProgressBar.localScale = new Vector3(resourceHarvester.GetNormalizedHarvestTimer(), 1, 1);
            harvestSpeedText.text = $"{resourceHarvester.GetHarvestAmountPerSecond():#.0} /s";
        }

        private void Update() {
            harvestProgressBar.localScale = new Vector3(1 - resourceHarvester.GetNormalizedHarvestTimer(), 1, 1);
            _timer -= Time.deltaTime;
            if (_timer > 0) return;
            _timer += _textUpdateTimer;
            harvestSpeedText.text = $"{resourceHarvester.GetHarvestAmountPerSecond():#.0} /s";
        }
    }
}