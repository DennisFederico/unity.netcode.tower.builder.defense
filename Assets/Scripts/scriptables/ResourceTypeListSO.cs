using System.Collections.Generic;
using UnityEngine;

namespace scriptables {
    [CreateAssetMenu(menuName = "ScriptableObjects/ResourceTypeList")]
    public class ResourceTypeListSO : ScriptableObject {
        public List<ResourceTypeSO> resourceTypeList;
    }
}