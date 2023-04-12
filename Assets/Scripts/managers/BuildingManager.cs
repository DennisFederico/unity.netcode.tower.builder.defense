using scriptables;
using UnityEngine;

namespace managers {
    public class BuildingManager : MonoBehaviour {
        public static BuildingManager Instance { get; private set; }

        [SerializeField] private Transform mouseSprite;
        [SerializeField] private BuildingTypeListSO buildingTypeList;
        private Camera _mainCamera;

        private void Awake() {
            //Singleton
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }
        }

        private void Start() {
            _mainCamera = Camera.main;
        }

        int buildingIndex = 0;

        void Update() {
            mouseSprite.position = GetWorldMousePosition();

            if (Input.GetMouseButtonDown(0)) {
                MultiplayerGameManager.Instance.SendBuildingSpawnRequest(buildingIndex, GetWorldMousePosition());
            }

            if (Input.GetKeyDown(KeyCode.Q)) {
                buildingIndex = 0;
            }

            if (Input.GetKeyDown(KeyCode.W)) {
                buildingIndex = 1;
            }

            if (Input.GetKeyDown(KeyCode.E)) {
                buildingIndex = 2;
            }

            // if (Input.GetMouseButtonDown(0)) {
            //     var worldMousePosition = GetWorldMousePosition();
            //     var hit = Physics2D.Raycast(worldMousePosition, Vector2.zero);
            //     if (hit.collider != null) {
            //         var building = hit.collider.GetComponent<Building>();
            //         if (building != null) {
            //             building.OnClick();
            //         }
            //     }
            // }
        }

        public Vector3 GetWorldMousePosition() {
            var screenToWorldPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            screenToWorldPoint.z = 0f;
            return screenToWorldPoint;
        }

        public BuildingTypeListSO GetBuildingTypeList() {
            return buildingTypeList;
        }
    }
}