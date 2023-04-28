using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour {
    [SerializeField] private Gradient gradient;
    [SerializeField] private float secondsPerDay = 10f;
    private Light2D _globalLight;
    private float _dayTime;
    private float _dayTimeSpeed;
    
    private void Awake() {
        _globalLight = GetComponent<Light2D>();
        _dayTimeSpeed = 1 / secondsPerDay;
    }

    private void Update() {
        _dayTime += Time.deltaTime * _dayTimeSpeed;
        _globalLight.color = gradient.Evaluate(_dayTime % 1f);
    }
}