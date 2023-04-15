using scriptables;
using Unity.Netcode;
using UnityEngine;

namespace resource {
    public class ResourceNode : NetworkBehaviour {
        [SerializeField] private ResourceTypeSO resourceType;
        [Space]
        [SerializeField] private int amount;
        public bool IsDepleted => amount <= 0;

        private void Awake() {
            amount = Random.Range(resourceType.nodeMinMaxAmount.x, resourceType.nodeMinMaxAmount.y);
        }

        public void Harvest(int amount) {
            this.amount -= amount;
            if (IsDepleted) {
                //TODO Destroy this node
            }
        }
    }
}