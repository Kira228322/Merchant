using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class UITabButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TabGroup _tabGroup;
    [HideInInspector] public Image Background;

    public void OnPointerClick(PointerEventData eventData)
    {
        _tabGroup.OnTabSelected(this);
    }

    private void Start()
    {
        Background = GetComponent<Image>();
        _tabGroup.Subscribe(this);
    }
}
