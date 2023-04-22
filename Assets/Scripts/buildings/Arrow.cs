using enemy;
using UnityEngine;
using utils;

namespace buildings {
    public class Arrow : MonoBehaviour {
        
        private const float MoveSpeed = 20f;
        private Enemy _targetEnemy;
        private Vector3 _moveDirection;
        private float _timeToLive = 3f;
        private const int HitDamage = 10;

        private void Update() {
            if (_targetEnemy) {
                _moveDirection = (_targetEnemy.transform.position - transform.position).normalized;
                transform.rotation = Quaternion.Euler(0f, 0f, _moveDirection.Get2DRotation());
            }
            // ReSharper disable once Unity.InefficientPropertyAccess
            transform.position += _moveDirection * (MoveSpeed * Time.deltaTime);
            HandleTieToLive();
        }
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.TryGetComponent<Enemy>(out var enemy)) {
                enemy.Damage(HitDamage);
                Destroy(gameObject);
            }
        }
        
        public void SetTargetEnemy(Enemy enemy) {
            _targetEnemy = enemy;
            _moveDirection = (_targetEnemy.transform.position - transform.position).normalized;
        }

        private void HandleTieToLive() {
            _timeToLive -= Time.deltaTime;
            if (_timeToLive <= 0f) {
                Destroy(gameObject);
            }
        }
    }
}