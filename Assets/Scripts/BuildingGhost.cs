using UnityEngine;
using utils;

public class BuildingGhost : MonoBehaviour {
    [SerializeField] private GameObject spriteGameObject;
    [SerializeField] private GameObject borderGameObject;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _borderRenderer;

    private void Awake() {
        _spriteRenderer = spriteGameObject.GetComponent<SpriteRenderer>();
        _borderRenderer = borderGameObject.GetComponent<SpriteRenderer>();
        Hide();
    }

    private void Update() {
        SetPosition(CursorManager.Instance.GetWorldMousePosition());
    }
    
    private void SetPosition(Vector3 position) {
        transform.position = position;
    }

    public void Show(Sprite ghostSprite) {
        _spriteRenderer.sprite = ghostSprite;
        spriteGameObject.SetActive(ghostSprite);
        borderGameObject.SetActive(ghostSprite);
    }
    
    private void Hide() {
        spriteGameObject.SetActive(false);
        borderGameObject.SetActive(false);
    }
    
    public void SetGhostColor(Color color) {
        _spriteRenderer.color = color;
    }
    
    public void SetBorderColor(Color color) {
        _borderRenderer.color = color;
    }
}
