using System.Collections;
using enemy;
using Unity.Netcode;
using UnityEngine;
using utils;
using Random = UnityEngine.Random;

namespace managers {
    public class EnemySpawner : NetworkBehaviour {
        public static EnemySpawner Instance { get; private set; }

        [SerializeField] private Transform enemyPrefab;
        [SerializeField] private Vector3 spawnPosition;
        [SerializeField] private int spawnAmount;

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }
        }

        // private void Start() {
        //     NetworkManager.Singleton.OnServerStarted += SpawnEnemyHorde;
        // }

        private void SpawnEnemyHorde() {
            StartCoroutine(SpawnEnemyHorde(spawnPosition, spawnAmount));
        }

        public IEnumerator SpawnEnemyHorde(Vector3 position, int amount) {
            Debug.Log($"Spawning enemy horde coroutine for {amount} enemies");
            int currentAmount = 0;
            while (currentAmount < amount) {
                int amountToSpawn = Mathf.Min(Random.Range(25, 40), amount - currentAmount);
                for (int i = 0; i < amountToSpawn; i++) {
                    Vector3 spawnPos = spawnPosition + Utils.GetRandomDirection() * 4f;
                    SpawnEnemy(spawnPos);
                    currentAmount++;
                }
                yield return new WaitForSeconds(0.1f);
            }
            Debug.Log("Spawning ended");
        }

        public Enemy SpawnEnemy(Vector3 position) {
            var enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
            enemy.GetComponent<NetworkObject>().Spawn(true);
            return enemy.GetComponent<Enemy>();
        }
    }
}