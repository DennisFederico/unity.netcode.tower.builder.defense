using Cinemachine;
using UnityEngine;

public class CameraHandler : MonoBehaviour {
    public static CameraHandler Instance { get; private set; }

    public bool EdgeScrollingEnabled {
        get => _edgeScrollingEnabled;
        set {
            _edgeScrollingEnabled = value;
            PlayerPrefs.SetInt("EdgeScrollingEnabled", value ? 1 : 0);
        }
    }

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float cameraSpeed = 50f;
    [SerializeField] private float edgeScrollingSize = 10f;
    private bool _edgeScrollingEnabled;
    private float _zoomAmount = 3f;
    private float _zoomSpeed = 30f;
    private (float min, float max) _zoomLimit = (10f, 40f);
    private float _ortographicSize;
    private float _targetOrthographicSize;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
        
        _edgeScrollingEnabled = PlayerPrefs.GetInt("EdgeScrollingEnabled", 1) == 1;
    }

    private void Start() {
        _ortographicSize = virtualCamera.m_Lens.OrthographicSize;
        _targetOrthographicSize = _ortographicSize;
    }

    private void LateUpdate() {
        HandleMovement();
        HandleZoom();
    }

    private void HandleZoom() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (EdgeScrollingEnabled) {
            if (Input.mousePosition.x >= Screen.width - edgeScrollingSize) {
                x = 1f;
            } else if (Input.mousePosition.x <= edgeScrollingSize) {
                x = -1f;
            }

            if (Input.mousePosition.y >= Screen.height - edgeScrollingSize) {
                y = 1f;
            } else if (Input.mousePosition.y <= edgeScrollingSize) {
                y = -1f;
            }
        }

        Vector3 moveDir = new Vector3(x, y).normalized;
        transform.position += moveDir * (cameraSpeed * Time.deltaTime);
    }

    private void HandleMovement() {
        _targetOrthographicSize -= Input.mouseScrollDelta.y * _zoomAmount;
        _targetOrthographicSize = Mathf.Clamp(_targetOrthographicSize, _zoomLimit.min, _zoomLimit.max);
        _ortographicSize = Mathf.MoveTowards(_ortographicSize, _targetOrthographicSize, _zoomSpeed * Time.deltaTime);

        virtualCamera.m_Lens.OrthographicSize = _ortographicSize;
    }
}