using managers;
using TMPro;
using UnityEngine;
using utils;

namespace ui {
    public class EnemyWaveUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI waveCountText;
        [SerializeField] private TextMeshProUGUI waveCountdownText;
        [SerializeField] private RectTransform wavePositionIndicator;
        [SerializeField] private RectTransform closestEnemyIndicator;
        private Camera _mainCamera;

        private void Start() {
            SetWaveCountText(EnemySpawner.Instance.GetCurrentHordeCount());
            EnemySpawner.Instance.OnNextHorde += SetWaveCountText;
            _mainCamera = Camera.main;
        }

        private void Update() {
            SetWaveCountdownText(EnemySpawner.Instance.GetTimeUntilNextHorde());
            HandleNextWaveIndicator();
            HandleClosestEnemyIndicator();
        }

        private void SetWaveCountText(int waveCount) {
            waveCountText.text = $"Wave {waveCount}";
        }

        private void SetWaveCountdownText(float waveCountdown) {
            if (waveCountdown < 0) {
                waveCountdownText.text = "Wave in progress";
            } else {
                waveCountdownText.text = $"Next wave in {waveCountdown:0.00}";
            }
        }

        private void HandleNextWaveIndicator() {
            var nextHordePosition = EnemySpawner.Instance.GetNextHordePosition();
            var cameraPosition = _mainCamera.transform.position;
            var dirToHorde = (nextHordePosition - cameraPosition).normalized;
            wavePositionIndicator.rotation = Quaternion.Euler(0f, 0f, dirToHorde.Get2DRotation());
            var indicatorDistance = Vector3.Distance(nextHordePosition, cameraPosition);
            wavePositionIndicator.gameObject.SetActive(indicatorDistance > _mainCamera.orthographicSize * 1.75f);
        }

        private void HandleClosestEnemyIndicator() {
            var closestEnemy = EnemySpawner.Instance.FindClosestEnemyFromPosition(_mainCamera.transform.position, 500f);
            closestEnemyIndicator.gameObject.SetActive(closestEnemy);
            if (!closestEnemy) return;
            var cameraPosition = _mainCamera.transform.position;
            var dirToEnemy = (closestEnemy.transform.position - cameraPosition).normalized;
            closestEnemyIndicator.rotation = Quaternion.Euler(0f, 0f, dirToEnemy.Get2DRotation());
            var indicatorDistance = Vector3.Distance(closestEnemy.position, cameraPosition);
            closestEnemyIndicator.gameObject.SetActive(indicatorDistance > _mainCamera.orthographicSize * 1.75f);
        }

        private void OnDestroy() {
            EnemySpawner.Instance.OnNextHorde -= SetWaveCountText;
        }
    }
}