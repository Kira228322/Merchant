using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonFunctionalWindow : MonoBehaviour
{
    [SerializeField] private GameObject _sleepPanelPrefab;
 
    public void OnSleepButtonClick()
    {
        Instantiate(_sleepPanelPrefab, MapManager.Canvas.transform);
        Destroy(gameObject);
    }

    public void OnSaveButtonClick()
    {
        GameManager.Instance.SaveGame();
        Destroy(gameObject);
    }

    public void OnCloseButtonClick()
    {
        Destroy(gameObject);
    }
}
