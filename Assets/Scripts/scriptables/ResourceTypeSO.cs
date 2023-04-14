using resource;
using UnityEngine;

namespace scriptables {
    [CreateAssetMenu(menuName = "ScriptableObjects/ResourceType")]
    public class ResourceTypeSO : ScriptableObject {
        public string resourceName;
        public Sprite sprite;
        public ResourceType resourceType;
    }
}