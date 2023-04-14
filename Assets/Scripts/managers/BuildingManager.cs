using System;
using scriptables;
using UnityEngine;
using UnityEngine.EventSystems;

namespace managers {
    public class BuildingManager : MonoBehaviour {
        public static BuildingManager Instance { get; private set; }
        public event Action<BuildingTypeSO> OnActiveBuildingTypeChanged; 

        [SerializeField] private Transform mouseSprite;
        [SerializeField] private BuildingTypeListSO buildingTypeList;
        private BuildingTypeSO _activeBuildingType;
        private int _buildingTypeIndex;
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

        void Update() {

            if (mouseSprite) mouseSprite.position = GetWorldMousePosition();
            
            if (Input.GetMouseButtonDown(0) &&_activeBuildingType && !EventSystem.current.IsPointerOverGameObject()) {
                MultiplayerGameManager.Instance.SendBuildingSpawnRequest(_buildingTypeIndex, GetWorldMousePosition());
            }
            
            if (Input.GetKeyDown(KeyCode.Escape)) {
                SetActiveBuildingType(null);
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
        
        public void SetActiveBuildingType(BuildingTypeSO buildingType) {
            _activeBuildingType = buildingType;
            _buildingTypeIndex = _activeBuildingType ? buildingTypeList.buildingTypeList.IndexOf(_activeBuildingType) : -1;
            if (mouseSprite) {
                mouseSprite.gameObject.SetActive(_activeBuildingType);
                mouseSprite.GetComponent<SpriteRenderer>().sprite = _activeBuildingType ? _activeBuildingType.sprite : null;
            }
            OnActiveBuildingTypeChanged?.Invoke(_activeBuildingType);
        }

        public BuildingTypeListSO GetBuildingTypeListSO() {
            return buildingTypeList;
        }
    }
}