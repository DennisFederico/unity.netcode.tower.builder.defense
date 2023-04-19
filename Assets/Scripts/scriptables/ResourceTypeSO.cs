using resource;
using UnityEngine;

namespace scriptables {
    [CreateAssetMenu(menuName = "ScriptableObjects/ResourceType")]
    public class ResourceTypeSO : ScriptableObject {
        public string resourceName;
        public string shortName;
        public Sprite sprite;
        public Color color;
        public ResourceType resourceType;
        public Vector2Int nodeMinMaxAmount;
    }
}