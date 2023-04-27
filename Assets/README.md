## Changes rolled back from Netcode attempt

### NetworkBootstrap.cs

```csharp
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
```

Reverted to MonoBehavior:
- Building.cs
- ResourceHarvester.cs
- ResourceNode
- EnemySpawner.cs
- ResourceManager.cs (Removed NetworkedList and handle manual notification of the change event)
- MultiplayerGameManager.cs

Prefabs (removed NetworkObject)
Managers:
- NetworkManager
- GameManager
- ResourceManager
- EnemySpawner

