using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    [RequireComponent(typeof(Image))]
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private Roman.Item _item;


        private Image _icon;
        private RectTransform _rectTransform;
        void Awake()
        {
            _icon = GetComponent<Image>();
            _rectTransform = GetComponent<RectTransform>();
            _icon.sprite = _item.Icon;
            gameObject.name = _item.Name;

        _rectTransform.sizeDelta = new Vector2(_item.CellSizeWidth * InventoryItemGrid.TileSizeWidth, _item.CellSizeHeight * InventoryItemGrid.TileSizeHeight);
        }

    }