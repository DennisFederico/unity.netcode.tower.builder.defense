using ParrelSync;
using Unity.Netcode;
using UnityEngine;

namespace utils {
    public class NetworkBootstrap : MonoBehaviour {
        // Start is called before the first frame update
        void Start() {
            if (ClonesManager.IsClone()) {
                NetworkManager.Singleton.StartClient();
            } else {
                NetworkManager.Singleton.StartHost();
            }
        }

        // Update is called once per frame
        void Update() { }
    }
}