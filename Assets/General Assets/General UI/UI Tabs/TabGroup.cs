using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    [SerializeField] private Sprite _tabIdle;
    [SerializeField] private Sprite _tabSelected;

    [SerializeField] private List<GameObject> _objectsToSwap;

    private List<UITabButton> _tabButtons;
    private UITabButton _selectedTab;

    public void Subscribe(UITabButton button)
    {
        if (_tabButtons == null)
        {
            _tabButtons = new List<UITabButton>();
        }

         _tabButtons.Add(button);
    }

    public void OnTabSelected(UITabButton button)
    {
        _selectedTab = button;
        ResetTabs();
        button.Background.sprite = _tabSelected;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < _objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                _objectsToSwap[i].SetActive(true);
            }
            else _objectsToSwap[i].SetActive(false);
        }
    }

    public void ResetTabs()
    {
        foreach (var button in _tabButtons)
        {
            if (button == _selectedTab) continue;
            button.Background.sprite = _tabIdle;
        }
    }
}
