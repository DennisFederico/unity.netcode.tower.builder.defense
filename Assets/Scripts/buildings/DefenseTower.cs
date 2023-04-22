using enemy;
using UnityEngine;

namespace buildings {
    public class DefenseTower : MonoBehaviour {
        [SerializeField]  Transform arrowPrefab;
        [SerializeField]  Transform arrowSpawnPoint;
        [SerializeField]  float attackCooldown = .2f;
        [SerializeField]  float attackRange = 10f;
        [SerializeField]  LayerMask enemyLayerMask;
        private float _attackCooldownTimer;
        private Enemy _currentTarget;
        
        private void Update() {
            _attackCooldownTimer -= Time.deltaTime;
            if (_attackCooldownTimer > 0f) return;
            _attackCooldownTimer += attackCooldown;
            HandleTargeting();
            HandleShooting();
        }

        #region Targeting
        private void HandleTargeting() {
            _currentTarget = TryFindClosestEnemyInRadius(out var target) ? target : null;
        }

        private bool TryFindClosestEnemyInRadius(out Enemy enemyTransform) {
            Collider2D closestCollider = null;
            var enemyColliders = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayerMask);
            float closestDistance = Mathf.Infinity;
            foreach (var aCollider in enemyColliders) {
                var distance = Vector3.Distance(transform.position, aCollider.transform.position);
                if (distance < closestDistance) {
                    closestDistance = distance;
                    closestCollider = aCollider;
                }
            }
            enemyTransform = closestCollider ? closestCollider.GetComponent<Enemy>() : null;
            return enemyTransform;
        }
        #endregion

        #region Shooting
        private void HandleShooting() {
            if (_currentTarget) {
                var arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
                arrow.GetComponent<Arrow>().SetTargetEnemy(_currentTarget);
            }
        }
        #endregion
    }
}