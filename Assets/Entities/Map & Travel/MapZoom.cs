using UnityEngine;
using UnityEngine.UI;

public class MapZoom : MonoBehaviour
{
    private float _minScale = 1;
    private float _maxScale = 2.2f;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private GameObject _content;
    private RectTransform _contentRectTransform;

    private void Start()
    {
        _contentRectTransform = _content.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevious = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevious = touchOne.position - touchOne.deltaPosition;

            float previousMagnitude = (touchZeroPrevious - touchOnePrevious).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = (currentMagnitude - previousMagnitude) * 0.005f;
            _contentRectTransform.localScale += new Vector3(difference, difference);
            if (_contentRectTransform.localScale.x > _maxScale)
            {
                _contentRectTransform.localScale = new Vector3(_maxScale, _maxScale);
            }
            else if (_contentRectTransform.localScale.x < _minScale)
                _contentRectTransform.localScale = new Vector3(_minScale, _minScale);

            if (_contentRectTransform.localScale.x > 1.1f)
                _scrollRect.enabled = true;
            else
            {
                _scrollRect.enabled = false;
                _contentRectTransform.position = new Vector3(Screen.width / 2, Screen.height / 2);
            }
        }
        
        
    }

    private void OnDisable()
    {
        _scrollRect.enabled = false;
        _contentRectTransform.position = new Vector3(Screen.width / 2, Screen.height / 2);
        _contentRectTransform.localScale = Vector3.one;
    }
}
