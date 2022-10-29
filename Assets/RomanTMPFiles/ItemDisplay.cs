using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    [RequireComponent(typeof(Image))]
    public class ItemDisplay : MonoBehaviour
    {
        [SerializeField] private Roman.Item _item;

        private Image _icon;
        private RectTransform _rectTransform;
        void Start()
        {
            _icon = GetComponent<Image>();
            _rectTransform = GetComponent<RectTransform>();
            _icon.sprite = _item.Icon;
            gameObject.name = _item.Name;

            _rectTransform.sizeDelta = new Vector2(_item.CellSizeWidth * 32, _item.CellSizeHeight * 32); //Умножить на 32 - костыль костыль пока что
        }

    }