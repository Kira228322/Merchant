using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class ItemDragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas _canvas;

    private Transform _previousParent;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
      //  _previousParent = transform.parent;
      //  transform.parent = null;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0.7f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // var result = new List<RaycastResult>();
        // EventSystem.current.RaycastAll(eventData, result);
        //  foreach(var raycast in result)
        // {
        //     if (raycast.gameObject.TryGetComponent(out InventoryItemGrid itemGrid))
        //     {
        //         transform.parent = raycast.gameObject.transform;
        //         //itemGrid логика
        //         return;
        //     }
        //     transform.parent = _previousParent;
        // }

        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;
    }

}
