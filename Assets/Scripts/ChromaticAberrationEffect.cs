using UnityEngine;
using UnityEngine.Rendering;

public class ChromaticAberrationEffect : MonoBehaviour {
    
    public static ChromaticAberrationEffect Instance { get; private set; }
    
    private Volume _volume;
    private float _decreaseSpeed = 1f;
    
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
        
        _volume = GetComponent<Volume>();
    }

    private void Update() {
        //_volume.weight = Mathf.Sin(Time.time * 2) * 0.5f + 0.5f;
        if (_volume.weight > 0) {
            _volume.weight -= Time.deltaTime * _decreaseSpeed;
        }
    }
    
    public void SetWeight(float amount) {
        _volume.weight = amount;
    }
}