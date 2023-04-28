using TMPro;
using UnityEngine;
using utils;

namespace ui {
    public class TooltipUI : MonoBehaviour {
        
        public static TooltipUI Instance { get; private set; }
        
        [SerializeField] private RectTransform parentCanvas;
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private RectTransform backgroundRectTransform;
        private RectTransform _rectTransform;
        private readonly Vector2 _textPadding = new Vector2(10, 0);
        private Timer _timer;

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }
            _rectTransform = GetComponent<RectTransform>();
            
            Hide();
        }

        private void LateUpdate() {
            HandleTooltipPosition();
            if (_timer == null) return;
            _timer.Update(Time.deltaTime);
            if (_timer.IsFinished()) {
                Hide();
            }
        }

        private void HandleTooltipPosition() {
            var anchoredPosition = Input.mousePosition / parentCanvas.localScale.x;
            anchoredPosition.x = Mathf.Min(anchoredPosition.x, parentCanvas.rect.width - backgroundRectTransform.rect.width);
            anchoredPosition.x = Mathf.Max(anchoredPosition.x, 0);
            anchoredPosition.y = Mathf.Min(anchoredPosition.y, parentCanvas.rect.height - backgroundRectTransform.rect.height);
            anchoredPosition.y = Mathf.Max(anchoredPosition.y, 0);
            _rectTransform.anchoredPosition = anchoredPosition;
        }
      
        public void Show(string message, float timer = 0) {
            _timer = timer > 0 ? new Timer(timer) : null;
            messageText.text = message;
            messageText.ForceMeshUpdate(true);
            backgroundRectTransform.sizeDelta = messageText.GetRenderedValues(true) + _textPadding;            
            gameObject.SetActive(true);
        }
        
        public void Hide() {
            gameObject.SetActive(false);
        }
    }
}