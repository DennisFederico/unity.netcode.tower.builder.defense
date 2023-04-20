using managers;
using UnityEngine;

namespace ui {

    public class HealthBarOverlay : MonoBehaviour {
        [SerializeField] private Transform healthBarTransform;
        private HealthSystem _healthSystem;

        private void Awake() {
            _healthSystem = GetComponentInParent<HealthSystem>();
        }

        private void Start() {
            _healthSystem.OnDamage += HandleDamage;
            UpdateBar();
        }

        private void HandleDamage() {
            UpdateBar();
        }

        private void UpdateBar() {
            healthBarTransform.localScale = new Vector3(_healthSystem.CurrentHealthNormalized, 1, 1);
            gameObject.SetActive(!_healthSystem.IsFullHealth);
        }
    }
}