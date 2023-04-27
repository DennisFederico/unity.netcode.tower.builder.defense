using System;
using managers;
using scriptables;
using UnityEngine;

namespace buildings {
    public class BuildingConstruction : MonoBehaviour {
        
        private float _constructionTimer;
        private float _constructionTimerMax;
        private int _buildingTypeIndex;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BoxCollider2D boxCollider2D;
        [SerializeField] private Transform buildingPlacedEffect;
        private Material _constructionMaterial;
        private static readonly int MaterialLabelProgress = Shader.PropertyToID("_Progress");

        public float ConstructionTimerNormalized => 1 - _constructionTimer / _constructionTimerMax;

        private void Awake() {
            _constructionMaterial = spriteRenderer.material;
        }

        private void Update() {
            _constructionTimer -= Time.deltaTime;
            _constructionMaterial.SetFloat(MaterialLabelProgress, ConstructionTimerNormalized);
            if (_constructionTimer <= 0f) {
                Instantiate(buildingPlacedEffect, transform.position, Quaternion.identity);
                MultiplayerGameManager.Instance.SendBuildingSpawnRequest(_buildingTypeIndex, transform.position);
                Destroy(gameObject);
            }
        }
        
        public void Setup(BuildingTypeSO buildingType, int buildingTypeIndex) {
            _constructionTimerMax = buildingType.constructionTime;
            _constructionTimer = _constructionTimerMax;
            spriteRenderer.sprite = buildingType.sprite;
            var buildingCollider = buildingType.buildingPrefab.GetComponent<BoxCollider2D>();
            boxCollider2D.size = buildingCollider.size;
            boxCollider2D.offset = buildingCollider.offset;
            _buildingTypeIndex = buildingTypeIndex;
        }
    }
}