using resource;
using UnityEngine;
using UnityEngine.Serialization;

namespace scriptables {
    [CreateAssetMenu(menuName = "ScriptableObjects/ResourceType")]
    public class ResourceTypeSO : ScriptableObject {
        public string resourceName;
        [FormerlySerializedAs("spriteIcon")] public Sprite iconSprite;
        public ResourceType resourceType;
    }
}