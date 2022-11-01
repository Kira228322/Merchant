using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ¬ этом классе будут все инициализации, которые необходимы только в самом начале игры
    // ¬роде как преимущественно он только дл€ статических классов, но потом посмотрим 
    
    [SerializeField] private string _travelingScene;
    [SerializeField] private SceneTransiter _loadScreen;
    [SerializeField] private GameObject _roadWindow;
    
    void Start()
    {
        MapManager.Init(_travelingScene, _loadScreen, _roadWindow);
    }

}
