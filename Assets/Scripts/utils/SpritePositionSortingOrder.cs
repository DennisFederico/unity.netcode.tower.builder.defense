using UnityEngine;

namespace utils {
    [ExecuteInEditMode]
    public class SpritePositionSortingOrder : MonoBehaviour {
        
        [SerializeField] private float offsetY = 0f;
        private SpriteRenderer _spriteRenderer;
        private bool _isStatic;
        private readonly int _sortingPrecision = 5;

        private void Awake() {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            var parent = transform.parent;
            if (Application.isPlaying) {
                _isStatic = gameObject.isStatic || (parent && parent.gameObject.isStatic);
            }
        }

        private void Start() {
            UpdateSortingOrder();
            if (_isStatic && Application.isPlaying) {
                Destroy(this);
            }
        }

        private void LateUpdate() {
            if (_isStatic) return;
            UpdateSortingOrder();
        }

        private void UpdateSortingOrder() {
            Debug.Log("UpdateSortingOrder");
            _spriteRenderer.sortingOrder = (int)((-transform.position.y + offsetY) * _sortingPrecision);
        }
    }
}