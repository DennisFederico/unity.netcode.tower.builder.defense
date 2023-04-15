using UnityEngine;
using utils;

public class BuildingGhost : MonoBehaviour {
    [SerializeField] private GameObject spriteGameObject;
    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _spriteRenderer = spriteGameObject.GetComponent<SpriteRenderer>();
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
        spriteGameObject.SetActive(ghostSprite? true : false);
    }
    
    public void Hide() {
        spriteGameObject.SetActive(false);
    }
}
