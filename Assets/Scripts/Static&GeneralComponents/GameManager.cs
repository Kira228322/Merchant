using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    // ¬ этом классе будут все инициализации, которые необходимы только в самом начале игры
    // ¬роде как преимущественно он только дл€ статических классов, но потом посмотрим 
    
    [SerializeField] private string _travelingScene;
    [SerializeField] private SceneTransiter _loadScreen;
    [SerializeField] private GameObject _roadWindow;
    [SerializeField] private GameObject _villageWindow;
    [SerializeField] private Canvas _canvas;
    [FormerlySerializedAs("TravelBlock")] [SerializeField] private GameObject _travelBlock;
    [FormerlySerializedAs("TravelTime")] [SerializeField] private TMP_Text _travelTime;
    [FormerlySerializedAs("Timeflow")] [SerializeField] private Timeflow _timeflow;
    [SerializeField] private TravelTimeCounter _timeCounter;
    [SerializeField] private GameObject _playerIcone;
        void Start()
    {
        MapManager.Init(_travelingScene, _loadScreen, _roadWindow, _villageWindow, _canvas);
        TravelManager.Init(_travelBlock, _travelTime, _timeCounter, _playerIcone);
        GameTime.Init(_timeflow);
    }

}
