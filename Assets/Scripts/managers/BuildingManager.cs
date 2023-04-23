using System;
using buildings;
using scriptables;
using ui;
using Unity.Mathematics;
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
        [SerializeField] private Transform buildingConstruction;
        
        [SerializeField] private Building hqBuilding;
        
        private BuildingTypeSO _activeBuildingType;
        private int _buildingTypeIndex;
        private Vector2 _spriteBoxSize;
        private Vector2 _spriteBoxOffset;
        private bool _isBuildingAreaClear;
        private bool _isBuildingTooClose;
        private bool _isBuildingTooFar;
        // private bool _canSpawnBuilding;
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
                _isBuildingTooClose = UpdateIsBuildingTooClose(worldPosition);
                _isBuildingTooFar = UpdateIsBuildingTooFar(worldPosition);
                
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
                    if (CanSpawnBuildingCheck(out var errorMessage)) {
                        CreateBuildingConstruction(worldPosition);
                        //MultiplayerGameManager.Instance.SendBuildingSpawnRequest(_buildingTypeIndex, worldPosition);
                    } else {
                        TooltipUI.Instance.Show(errorMessage, new TooltipUI.TooltipTimer(1.5f));
                    }
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
        
        public Building GetHQBuilding() {
            return hqBuilding;
        }

        private bool CanSpawnBuildingCheck(out string errorMessage) {
            if (!_isBuildingAreaClear) {
                errorMessage = "Area is not clear";
                return false;
            }
            if (_isBuildingTooClose) {
                errorMessage = "Too close to other buildings";
                return false;
            }
            if (_isBuildingTooFar) {
                errorMessage = "Too far from other buildings";
                return false;
            }
            if (!ResourceManager.Instance.TrySpendResources(_activeBuildingType.resourceCost)) {
                errorMessage = $"Not enough resources\n{_activeBuildingType.GetBuildingCostString()}";
                return false;
            }
            errorMessage = "";
            return true;
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
            // ReSharper disable Unity.PreferNonAllocApi
            var overlapBoxAll = Physics2D.OverlapBoxAll(position + _spriteBoxOffset, _spriteBoxSize, 0f);
            return overlapBoxAll.Length == 0;
            // ReSharper restore Unity.PreferNonAllocApi
        }
        
        public bool IsBuildAreaClear() {
            return _activeBuildingType && _isBuildingAreaClear;
        }
        
        private bool UpdateIsBuildingTooClose(Vector2 position) {
            // ReSharper disable Unity.PreferNonAllocApi
            var tooCloseBuildings = Physics2D.OverlapBoxAll(position + _spriteBoxOffset, _spriteBoxSize + _activeBuildingType.safeFromBuildingsDistance, 0f, buildingLayer);
            return tooCloseBuildings.Length > 0;
            // ReSharper restore Unity.PreferNonAllocApi
        }

        public bool IsBuildingTooClose() {
            return _activeBuildingType && _isBuildingTooClose;
        }
        
        private bool UpdateIsBuildingTooFar(Vector2 position) {
            // ReSharper disable Unity.PreferNonAllocApi
            var maxFarBuildings = Physics2D.OverlapCircleAll(position, _activeBuildingType.maxDistanceFromBuildings, buildingLayer);
            return maxFarBuildings.Length == 0;
            // ReSharper restore Unity.PreferNonAllocApi
        }

        public bool IsBuildingTooFar() {
            return _activeBuildingType && _isBuildingTooFar;
        }
        
        private BuildingConstruction CreateBuildingConstruction(Vector3 worldPosition) {
            var aTransform = Instantiate(this.buildingConstruction, worldPosition, quaternion.identity);
            var aBuildingConstruction = aTransform.GetComponent<BuildingConstruction>();
            aBuildingConstruction.Setup(_activeBuildingType, _buildingTypeIndex);
            return aBuildingConstruction;
        }
    }
}