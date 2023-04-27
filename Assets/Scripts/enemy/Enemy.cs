using System.Collections;
using buildings;
using managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace enemy {
    
    [RequireComponent(typeof(HealthSystem))]
    public class Enemy : MonoBehaviour {
        private Transform _currentTarget;
        private Rigidbody2D _rigidbody2D;
        private float _speed = 5f;
        private int _explosionDamage = 10;
        private float _targetRadius = 10f;
        private float _lookForTargetTimer;
        private float _lookForTargetTimerMax = .25f;
        [SerializeField] private int maxHealth = 30;
        private HealthSystem _healthSystem;
        [SerializeField] private GameObject onDieEffect;
        
        //TODO Move out of here and perhaps assign from the spawner
        [SerializeField] private LayerMask targetLayerMask;
        [SerializeField] private bool useTimer = true;

        private void Awake() {
            _healthSystem = GetComponent<HealthSystem>();
            _healthSystem.Initialize(maxHealth);
            _healthSystem.OnDie += () => {
                SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
                Instantiate(onDieEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            };
        }

        private void Start() {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _lookForTargetTimerMax = Random.Range(_lookForTargetTimerMax, _lookForTargetTimerMax * 1.25f);
            if (!useTimer) {
                StartCoroutine(LockOnClosestTarget(_lookForTargetTimerMax));
            }
        }

        private void Update() {
            HandleTargeting();
            HandleMoving();
        }

        private void HandleTargeting() {
            if (!useTimer) return;
            _lookForTargetTimer -= Time.deltaTime;
            if (_lookForTargetTimer > 0f) return;
            _currentTarget = TryLookForTargetInRadius(out var target) ? target : BuildingManager.Instance.GetHqBuilding()?.transform;
            _lookForTargetTimer += _lookForTargetTimerMax;
        }

        private void HandleMoving() {
            if (!_currentTarget) return;
            var direction = (_currentTarget.position - transform.position).normalized;
            _rigidbody2D.velocity = direction * _speed;
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            var building = collision.gameObject.GetComponent<Building>();
            //TODO - Check the building owner
            if (building) {
                building.GetComponent<HealthSystem>().Damage(_explosionDamage);
                SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDamaged);
                //FORCE THE HEALTH SYSTEM TO DIE
                _healthSystem.Damage(999);
            }
        }

        private IEnumerator LockOnClosestTarget(float delay = 0.250f) {
            while (true) {
                _currentTarget = TryLookForTargetInRadius(out var target) ? target : BuildingManager.Instance.GetHqBuilding()?.transform;
                yield return new WaitForSeconds(delay);
            }
            // ReSharper disable once IteratorNeverReturns
        }

        private bool TryLookForTargetInRadius(out Transform target) {
            // ReSharper disable once Unity.PreferNonAllocApi
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _targetRadius, targetLayerMask);
            Transform closetTarget = null;
            float closestDistance = Mathf.Infinity;
            foreach (var aCollider in colliders) {
                var distance = Vector3.Distance(transform.position, aCollider.transform.position);
                if (distance < closestDistance) {
                    closestDistance = distance;
                    closetTarget = aCollider.transform;
                }
            }

            target = closetTarget;
            return closetTarget;
        }
        
        public void Damage(int damage) {
            SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
            _healthSystem.Damage(damage);
        }
    }
}