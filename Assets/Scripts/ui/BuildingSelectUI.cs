using System.Collections.Generic;
using managers;
using scriptables;
using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public class BuildingSelectUI : MonoBehaviour {
        [SerializeField] private Transform buildingSelectUIBtnPrefab;
        private readonly Dictionary<BuildingTypeSO, BuildingSelectUIBtn> _buildingSelectUIBtnDict = new();
        private BuildingSelectUIBtn _buildingUnSelectBtn;

        private void Start() {
            //Add unselect button
            var unSelectButton = Instantiate(buildingSelectUIBtnPrefab, transform);
            unSelectButton.GetComponent<Button>().onClick.AddListener(() => { BuildingManager.Instance.SetActiveBuildingType(null); });
            _buildingUnSelectBtn = unSelectButton.GetComponent<BuildingSelectUIBtn>();
            _buildingUnSelectBtn.SetSelected(true);

            //Add buttons for each building type
            foreach (var buildingType in BuildingManager.Instance.GetBuildingTypeListSO().buildingTypeList) {
                var buildingSelectUIBtn = Instantiate(buildingSelectUIBtnPrefab, transform);
                var selectUIBtn = buildingSelectUIBtn.GetComponent<BuildingSelectUIBtn>();
                selectUIBtn.Initialize(buildingType);
                buildingSelectUIBtn.GetComponent<Button>().onClick.AddListener(() => { BuildingManager.Instance.SetActiveBuildingType(buildingType); });
                var mouseEnterExitEvents = buildingSelectUIBtn.GetComponent<MouseEnterExitEvents>();
                mouseEnterExitEvents.OnMouseEnter += () => TooltipUI.Instance.Show(GetTooltipText(buildingType));
                mouseEnterExitEvents.OnMouseExit += () => TooltipUI.Instance.Hide();
                _buildingSelectUIBtnDict[buildingType] = selectUIBtn;
                _buildingSelectUIBtnDict[buildingType] = selectUIBtn;
            }

            BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
        }

        private string GetTooltipText(BuildingTypeSO buildingType) {
            return $"{buildingType.buildingName}\n{buildingType.GetBuildingCostString()}";
        }

        private void BuildingManager_OnActiveBuildingTypeChanged(BuildingTypeSO buildingType) {
            UnSelectAll();
            if (buildingType) {
                if (_buildingSelectUIBtnDict.TryGetValue(buildingType, out var button)) {
                    button.SetSelected(true);
                }
            } else {
                _buildingUnSelectBtn.SetSelected(true);
            }
        }

        private void UnSelectAll() {
            _buildingUnSelectBtn.SetSelected(false);
            foreach (var buildingSelectUIBtn in _buildingSelectUIBtnDict.Values) {
                buildingSelectUIBtn.SetSelected(false);
            }
        }
    }
}