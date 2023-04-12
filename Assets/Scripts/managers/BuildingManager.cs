using scriptables;
using UnityEngine;

namespace managers {
    public class BuildingManager : MonoBehaviour {
        
        public static BuildingManager Instance { get; private set; }
        
        [SerializeField] private Transform mouseSprite;
        private Camera _mainCamera;
        private BuildingTypeListSO _buildingTypeList;

        private void Awake() {
            //Singleton
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }
            _buildingTypeList = Resources.Load<BuildingTypeListSO>(nameof(BuildingTypeListSO));
        }

        private void Start() {
            _mainCamera = Camera.main;
        }

        void Update() {
            mouseSprite.position = GetWorldMousePosition();
            
            if (!Input.GetMouseButtonDown(0)) return;
            MultiplayerGameManager.Instance.SendBuildingSpawnRequest(1, GetWorldMousePosition());
            //Instantiate(woodHarvesterPrefab, GetWorldMousePosition(), quaternion.identity);
            
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
            return _buildingTypeList;
        }
    }
}