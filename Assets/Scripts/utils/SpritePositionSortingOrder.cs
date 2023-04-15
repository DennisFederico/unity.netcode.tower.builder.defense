using UnityEngine;

namespace utils {
    [ExecuteInEditMode]
    public class SpritePositionSortingOrder : MonoBehaviour {
        
        [SerializeField] private float offsetY;
        [SerializeField] private bool runOnce;
        private SpriteRenderer _spriteRenderer;
        private bool _isStatic;
        private readonly int _sortingPrecision = 5;

        private void Awake() {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            var parent = transform.parent;
            if (Application.isPlaying) {
                _isStatic = gameObject.isStatic || AnyStaticParent(parent);
            }
        }

        private void Start() {
            UpdateSortingOrder();
            if ((_isStatic || runOnce) && Application.isPlaying) {
                Destroy(this);
            }
        }

        private void LateUpdate() {
            if (_isStatic) return;
            UpdateSortingOrder();
        }
        
        private static bool AnyStaticParent(Transform pTransform) {
            if (pTransform == null) return false;
            if (pTransform.gameObject.isStatic) return true;
            return AnyStaticParent(pTransform.parent);
        }

        private void UpdateSortingOrder() {
            _spriteRenderer.sortingOrder = (int)((-transform.position.y + offsetY) * _sortingPrecision);
        }
    }
}