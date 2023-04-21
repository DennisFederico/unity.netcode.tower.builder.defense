using ParrelSync;
using Unity.Netcode;
using UnityEngine;

namespace utils {
    public class NetworkBootstrap : MonoBehaviour {
        // Start is called before the first frame update
        void Start() {
            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
            if (ClonesManager.IsClone()) {
                NetworkManager.Singleton.StartClient();
            } else {
                NetworkManager.Singleton.StartHost();
            }
        }

        private void OnServerStarted() {
            Debug.Log("[Bootstrap] Server started!!!");
        }
    }
}