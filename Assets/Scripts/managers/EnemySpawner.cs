using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;
using utils;
using Random = UnityEngine.Random;

namespace managers {
    public class EnemySpawner : NetworkBehaviour {

        private enum State {
            WaitingForNextHorde,
            SpawningHorde,
        }
        
        public static EnemySpawner Instance { get; private set; }
        public event Action<int> OnNextHorde;
        [SerializeField] private List<Transform> spawnPositions;
        [SerializeField] private Transform enemyPrefab;
        [SerializeField] private Transform nextHordeIndicator;
        [SerializeField] private LayerMask enemyLayerMask;
        private int _startHordeAmount = 5;
        private int _extraHordeAmount = 3;
        private const float TimeBetweenHordes = 10f;
        private float _spawnHordeTimer;
        private State _state;
        private int _currentHorde;
        private Vector3 _nextHordePosition;
        
        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }
        }

        private void Start() {
            _state = State.WaitingForNextHorde;
            _spawnHordeTimer = TimeBetweenHordes;
            NextHordePosition();
        }

        private void Update() {
            switch (_state) {
                case State.WaitingForNextHorde:
                    _spawnHordeTimer -= Time.deltaTime;
                    if (_spawnHordeTimer <= 0) {
                        _spawnHordeTimer += TimeBetweenHordes;
                        _state = State.SpawningHorde;
                        SpawnEnemyHorde(_nextHordePosition);
                    }
                    break;
                case State.SpawningHorde:
                    break;
            }
        }

        private void NextHordePosition() {
            _nextHordePosition = spawnPositions[Random.Range(0, spawnPositions.Count)].position;
            nextHordeIndicator.position = _nextHordePosition;
            nextHordeIndicator.gameObject.SetActive(true);
        }
        
        private void SpawnEnemyHorde(Vector3 position) {
            nextHordeIndicator.gameObject.SetActive(false);
            var thisHordeAmount = (_currentHorde == 0 ? 50 : _startHordeAmount) + (_extraHordeAmount * _currentHorde);
            StartCoroutine(SpawnEnemyHorde(position, thisHordeAmount));
            _currentHorde++;
            OnNextHorde?.Invoke(_currentHorde);
        }

        public IEnumerator SpawnEnemyHorde(Vector3 position, int amount) {
            int currentAmount = 0;
            while (currentAmount < amount) {
                int amountToSpawn = Mathf.Min(Random.Range(1, 10), amount - currentAmount);
                for (int i = 0; i < amountToSpawn; i++) {
                    SpawnEnemy(position + Utils.GetRandomDirection() * Random.Range(0f, 4f));
                    currentAmount++;
                }
                yield return new WaitForSeconds(0.1f);
            }
            NextHordePosition();
            _state = State.WaitingForNextHorde;
        }

        private void SpawnEnemy(Vector3 position) {
            var enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
            enemy.GetComponent<NetworkObject>().Spawn(true);
        }

        public int GetCurrentHordeCount() {
            return _currentHorde;
        }
        
        public float GetTimeUntilNextHorde() {
            return _spawnHordeTimer;
        }
        
        public Vector3 GetNextHordePosition() {
            return _nextHordePosition;
        }
        
        private Collider2D[] _results = new Collider2D[50];
        public Transform FindClosestEnemyFromPosition(Vector3 position, float range) {
            Transform closestTransform = null;
            var size = Physics2D.OverlapCircleNonAlloc(position, range, _results, enemyLayerMask);
            float closestDistance = Mathf.Infinity;
            for (int i = 0; i < size; i++) {
                var aTransform = _results[i].transform;            
                var distance = Vector3.Distance(position, aTransform.position);
                if (distance < closestDistance) {
                    closestDistance = distance;
                    closestTransform = aTransform;
                }
            }
            return closestTransform;
        }
    }
}