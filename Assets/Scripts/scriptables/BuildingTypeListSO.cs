using System.Collections.Generic;
using UnityEngine;

namespace scriptables {
    [CreateAssetMenu(menuName = "ScriptableObjects/BuildingTypeList")]
    public class BuildingTypeListSO : ScriptableObject {
        public List<BuildingTypeSO> buildingTypeList;
        
        public BuildingTypeSO GetBuildingTypeByIndex(int index) {
            return buildingTypeList[index];
        }
        
        public Transform GetBuildingPrefabByIndex(int index) {
            return buildingTypeList[index].buildingPrefab;
        }
    }
}