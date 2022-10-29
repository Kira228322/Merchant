using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapButton : MonoBehaviour
{
    [SerializeField] private Image _map;

    public void OnMapButtonClick()
    {
        _map.gameObject.SetActive(!_map.gameObject.activeSelf);
    }
}
