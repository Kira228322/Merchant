using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonFunctionalWindow : MonoBehaviour
{
    [SerializeField] private GameObject _sleepPanelPrefub;
 
    public void OnSleepButtonClick()
    {
        Instantiate(_sleepPanelPrefub, MapManager.Canvas.transform);
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
