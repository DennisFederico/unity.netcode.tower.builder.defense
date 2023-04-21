using UnityEngine;

namespace utils {
    public class CursorManager : MonoBehaviour {
        
        public static CursorManager Instance { get; private set; }
        private Camera _mainCamera;

        
        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }
            _mainCamera = Camera.main;
        }
        public Vector3 GetWorldMousePosition() {
            var screenToWorldPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            screenToWorldPoint.z = 0f;
            return screenToWorldPoint;
        }
    }
}