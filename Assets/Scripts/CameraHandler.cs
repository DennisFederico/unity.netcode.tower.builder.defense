using Cinemachine;
using UnityEngine;

public class CameraHandler : MonoBehaviour {
    
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float cameraSpeed = 50f;
    private float _zoomAmount = 3f;
    private float _zoomSpeed = 30f;
    private (float min, float max) _zoomLimit = (10f, 40f);
    private float _ortographicSize;
    private float _targetOrthographicSize;
    
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