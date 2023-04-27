using scriptables;
using UnityEngine;

namespace resource {
    // public class ResourceNode : NetworkBehaviour {
    public class ResourceNode : MonoBehaviour {
        [SerializeField] private ResourceTypeSO resourceType;
        [Space]
        [SerializeField] private int amount;
        public bool IsDepleted => amount <= 0;

        private void Awake() {
            amount = Random.Range(resourceType.nodeMinMaxAmount.x, resourceType.nodeMinMaxAmount.y);
        }

        public void Harvest(int anAmount) {
            amount -= anAmount;
            if (IsDepleted) {
                //TODO Destroy this node
            }
        }
    }
}