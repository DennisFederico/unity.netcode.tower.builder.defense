using Cinemachine;
using UnityEngine;

public class CineMachineShake : MonoBehaviour {
    
    public static CineMachineShake Instance { get; private set; }
    
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineBasicMultiChannelPerlin _perlinNoise;
    private float _timer;
    private float _timerMax;
    private float _startingIntensity;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
        
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _perlinNoise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update() {
        if (_timer < _timerMax) {
            _timer += Time.deltaTime;
            float amplitude = Mathf.Lerp(_startingIntensity, 0f, _timer / _timerMax);
            _perlinNoise.m_AmplitudeGain = amplitude;
        }
    }

    public void ShakeCamera(float intensity, float timerMax) {
        _timerMax = timerMax;
        _timer = 0f;
        _startingIntensity = intensity;
        _perlinNoise.m_AmplitudeGain = intensity;
    }
    
}