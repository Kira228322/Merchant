using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaceOnMap : MonoBehaviour
{
    [SerializeField] private string _travelingScene; // Но лучше будет сделать какой-то MapManager в котором
                                                     // будет будет ссылка на эту сцену поездки и другие функции,
                                                     // чтобы не давать одну и туже ссылку каждому Plac'у на карте  
    
    [SerializeField] private bool _playerIsHere; // Потом это поле будет не сериалайз

    public void OnPlaceClick()
    {
        if (_playerIsHere)
            return;

        SceneManager.LoadSceneAsync(_travelingScene);
        // анимация затухания экрана и загрузки
    }
}
