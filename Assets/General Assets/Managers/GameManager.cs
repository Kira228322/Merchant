using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    // В этом классе будут все инициализации, которые необходимы только в самом начале игры
    // Вроде как преимущественно он только для статических классов, но потом посмотрим 
    [SerializeField] private Canvas _canvas;
    [SerializeField] private PlayersInventory _playersInventory;
    [Header("MapManager")] 
    [SerializeField] private string _travelingScene;
    [SerializeField] private SceneTransiter _loadScreen;
    [SerializeField] private GameObject _roadWindow;
    [SerializeField] private GameObject _villageWindow;
    
    [Header("TravelManager")] 
    [FormerlySerializedAs("TravelBlock")] [SerializeField] private GameObject _travelBlock;
    [FormerlySerializedAs("TravelTime")] [SerializeField] private TMP_Text _travelTime;
    [SerializeField] private TravelTimeCounter _timeCounter;
    [SerializeField] private GameObject _playerIcone;
    
    [Header("GameTime")] 
    [FormerlySerializedAs("Timeflow")] [SerializeField] private Timeflow _timeflow;
    
    [Header("TradeManager")] 
    //Ахтунг, нарушение конвенции именования! Бан
    [SerializeField] private GameObject TraderPanel;
    [SerializeField] private GameObject SellPanel;
    [SerializeField] private Transform TraderPanelContent;
    [SerializeField] private GoodsPanel GoodsPanelPrefab;

    void Start()
    {
        MapManager.Init(_travelingScene, _loadScreen, _roadWindow, _villageWindow, _canvas);
        TravelManager.Init(_travelBlock, _travelTime, _timeCounter, _playerIcone);
        GameTime.Init(_timeflow);
        TradeManager.Init(TraderPanel, SellPanel, TraderPanelContent, GoodsPanelPrefab, _playersInventory);
    }

}
