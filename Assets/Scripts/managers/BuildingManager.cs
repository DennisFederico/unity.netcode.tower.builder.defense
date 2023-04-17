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
        [SerializeField] private LayerMask buildingLayer;
        private BuildingTypeSO _activeBuildingType;
        private int _buildingTypeIndex;
        private Vector2 _spriteBoxSize;
        private Vector2 _spriteBoxOffset;
        private bool _isBuildingAreaClear;
        private bool _isBuildingAreaSafe;
        private bool _canSpawnBuilding;
        private readonly float _ghostUpdateTimer = .5f;
        private float _timer;


        private void Awake() {
            //Singleton
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }
        }

        void Update() {
            if (_activeBuildingType) {
                var worldPosition = CursorManager.Instance.GetWorldMousePosition();
                _isBuildingAreaClear = UpdateIsBuildAreaClear(worldPosition);
                _isBuildingAreaSafe = UpdateIsBuildAreaSafe(worldPosition);
                _canSpawnBuilding = _isBuildingAreaClear && _isBuildingAreaSafe;

                if (Input.GetMouseButtonDown(0) &&
                    _canSpawnBuilding &&
                    !EventSystem.current.IsPointerOverGameObject()) {
                    MultiplayerGameManager.Instance.SendBuildingSpawnRequest(_buildingTypeIndex, worldPosition);
                }
                
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    SetActiveBuildingType(null);
                }
            } else {
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
        }

        private void LateUpdate() {
            if (!_activeBuildingType) return;
            
            _timer -= Time.deltaTime;
            if (_timer > 0) return;
            _timer = _ghostUpdateTimer;
            mouseGhost.UpdateVisuals();
        }

        public BuildingTypeListSO GetBuildingTypeListSO() {
            return buildingTypeList;
        }

        public void SetActiveBuildingType(BuildingTypeSO buildingType) {
            _activeBuildingType = buildingType;
            _buildingTypeIndex = _activeBuildingType ? buildingTypeList.buildingTypeList.IndexOf(_activeBuildingType) : -1;

            if (_activeBuildingType && _activeBuildingType.buildingPrefab.TryGetComponent<BoxCollider2D>(out var boxCollider2D)) {
                _spriteBoxSize = boxCollider2D.size;
                _spriteBoxOffset = boxCollider2D.offset;
            }

            if (_activeBuildingType) {
                mouseGhost.Show(_activeBuildingType);
            } else {
                mouseGhost.Hide();
            }

            OnActiveBuildingTypeChanged?.Invoke(_activeBuildingType);
        }
        
        private bool UpdateIsBuildAreaClear(Vector2 position) {
            if (!_activeBuildingType) return false;
            // ReSharper disable Unity.PreferNonAllocApi
            var overlapBoxAll = Physics2D.OverlapBoxAll(position + _spriteBoxOffset, _spriteBoxSize, 0f);
            return overlapBoxAll.Length == 0;
            // ReSharper restore Unity.PreferNonAllocApi
        }
        
        public bool IsBuildAreaClear() {
            return _activeBuildingType && _isBuildingAreaClear;
        }
        
        private bool UpdateIsBuildAreaSafe(Vector2 position) {
            if (!_activeBuildingType) return false;
            // ReSharper disable Unity.PreferNonAllocApi
            var tooCloseBuildings = Physics2D.OverlapBoxAll(position + _spriteBoxOffset, _spriteBoxSize + _activeBuildingType.safeFromBuildingsDistance, 0f, buildingLayer);
            var maxFarBuildings = Physics2D.OverlapCircleAll(position, _activeBuildingType.maxDistanceFromBuildings, buildingLayer);
            return tooCloseBuildings.Length == 0 && maxFarBuildings.Length > 0;
            // ReSharper restore Unity.PreferNonAllocApi
        }

        public bool IsBuildAreaSafe() {
            return _activeBuildingType && _isBuildingAreaClear;
        }
    }
}