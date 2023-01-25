using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapZoom : MonoBehaviour
{
    private float _minScale = 1;
    private float _maxScale = 1.8f;
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

            float difference = (currentMagnitude - previousMagnitude) * 0.01f;
            transform.localScale += new Vector3(difference, difference);
            if (transform.localScale.x > _maxScale)
            {
                transform.localScale = new Vector3(_maxScale, _maxScale);
            }
            else if (transform.localScale.x < _minScale)
                transform.localScale = new Vector3(_minScale, _minScale);

            if (transform.localScale.x > 1.1f)
                _scrollRect.enabled = true;
            else
            {
                _scrollRect.enabled = false;
                _contentRectTransform.position = new Vector3(Screen.width/2, Screen.height/2);
            }

        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            transform.localScale += new Vector3(0.03f, 0.03f);
            if (transform.localScale.x > _maxScale)
            {
                transform.localScale = new Vector3(_maxScale, _maxScale);
                return;
            }
            if (transform.localScale.x < _minScale)
                transform.localScale = new Vector3(_minScale, _minScale);
            if (transform.localScale.x > 1.1f)
                _scrollRect.enabled = true;
            else
            {
                _scrollRect.enabled = false;
                _contentRectTransform.position = new Vector3(Screen.width/2, Screen.height/2);
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            transform.localScale -= new Vector3(0.03f, 0.03f);
            if (transform.localScale.x > _maxScale)
            {
                transform.localScale = new Vector3(_maxScale, _maxScale);
                return;
            }
            if (transform.localScale.x < _minScale)
                transform.localScale = new Vector3(_minScale, _minScale);
            if (transform.localScale.x > 1.1f)
                _scrollRect.enabled = true;
            else
            {
                _scrollRect.enabled = false;
                _contentRectTransform.position = new Vector3(Screen.width/2, Screen.height/2);
            }
        }
        
    }

    private void OnDisable()
    {
        _scrollRect.enabled = false;
        _contentRectTransform.position = new Vector3(Screen.width/2, Screen.height/2);
        transform.localScale = Vector3.one;
    }
}
