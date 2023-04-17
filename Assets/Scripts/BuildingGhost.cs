using managers;
using scriptables;
using ui;
using UnityEngine;
using utils;

public class BuildingGhost : MonoBehaviour {
    [SerializeField] private GameObject spriteGameObject;
    [SerializeField] private GameObject borderGameObject;
    [SerializeField] private ResourceNearbyOverlay infoOverlay;
    [SerializeField] private Color canBuildColor;
    [SerializeField] private Color cannotBuildColor;
    [SerializeField] private Color safeBuildColor;
    private BuildingTypeSO _buildingType;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _borderRenderer;
    private bool _isShowing;
    

    private void Awake() {
        _spriteRenderer = spriteGameObject.GetComponent<SpriteRenderer>();
        _borderRenderer = borderGameObject.GetComponent<SpriteRenderer>();
        Hide();
    }

    private void Update() {
        if (!_isShowing) return;
        transform.position = CursorManager.Instance.GetWorldMousePosition();
        UpdateVisuals();
    }

    public void UpdateVisuals() {
        _spriteRenderer.color = BuildingManager.Instance.IsBuildAreaClear() ? canBuildColor : cannotBuildColor;
        _borderRenderer.color = BuildingManager.Instance.IsBuildAreaSafe() ? safeBuildColor : cannotBuildColor;
        infoOverlay.UpdateText(_buildingType.resourceHarvestData);
    }

    public void Show(BuildingTypeSO buildingType) {
        _buildingType = buildingType;
        _spriteRenderer.sprite = buildingType.sprite;
        spriteGameObject.SetActive(true);
        borderGameObject.SetActive(true);
        infoOverlay.Show(buildingType.resourceHarvestData);
        _isShowing = true;
    }
    
    public void Hide() {
        _spriteRenderer.sprite = null;
        spriteGameObject.SetActive(false);
        borderGameObject.SetActive(false);
        infoOverlay.Hide();
        _isShowing = false;
    }
}
