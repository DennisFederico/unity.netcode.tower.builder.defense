using scriptables;
using UnityEngine;

namespace buildings {
    public class BuildingTypeHolder : MonoBehaviour {
        [SerializeField] private BuildingTypeSO buildingTypeSO;
        public BuildingTypeSO BuildingType => buildingTypeSO;
    }
}