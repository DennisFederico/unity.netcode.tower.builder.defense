using managers;
using UnityEngine;

namespace scriptables {
    [CreateAssetMenu(menuName = "ScriptableObjects/ResourceType")]
    public class ResourceTypeSO : ScriptableObject {
        public string resourceName;
        public ResourceManager.ResourceType resourceType;
    }
}