using System;
using scriptables;
using UnityEngine;
using UnityEngine.EventSystems;
using utils;

namespace managers {
    public class BuildingManager : MonoBehaviour {
        public static BuildingManager Instance { get; private set; }
        public event Action<BuildingTypeSO> OnActiveBuildingTypeChanged;

        [SerializeField] private BuildingGhost mouseGhost;
        [SerializeField] private BuildingTypeListSO buildingTypeList;
        private BuildingTypeSO _activeBuildingType;
        private int _buildingTypeIndex;

        private void Awake() {
            //Singleton
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }
        }

        void Update() {
            if (Input.GetMouseButtonDown(0) &&_activeBuildingType && !EventSystem.current.IsPointerOverGameObject()) {
                MultiplayerGameManager.Instance.SendBuildingSpawnRequest(_buildingTypeIndex, CursorManager.Instance.GetWorldMousePosition());
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

        public void SetActiveBuildingType(BuildingTypeSO buildingType) {
            _activeBuildingType = buildingType;
            _buildingTypeIndex = _activeBuildingType ? buildingTypeList.buildingTypeList.IndexOf(_activeBuildingType) : -1;
            if (mouseGhost) {
                mouseGhost.Show(_activeBuildingType? _activeBuildingType.sprite : null);
            }
            OnActiveBuildingTypeChanged?.Invoke(_activeBuildingType);
        }

        public BuildingTypeListSO GetBuildingTypeListSO() {
            return buildingTypeList;
        }
    }
}