using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusPanelRayCastTargeting : MonoBehaviour
{
    [SerializeField] private Image _scrollView;
    [SerializeField] private Image _viewPort;

    public void ChangeEnable(bool enable)
    {
        _scrollView.raycastTarget = enable;
        _viewPort.raycastTarget = enable;
    }
}
