using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class RandomBGGenerator : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private List<GameObject> _trees = new List<GameObject>();
    [SerializeField] private float _minTimeTreeSpawn;
    private float _lastTimeTreeSpawn;
    [SerializeField] private List<GameObject> _bush = new List<GameObject>();
    [SerializeField] private float _minTimeBushSpawn;
    private float _lastTimeBushSpawn;
    [SerializeField] private List<GameObject> _stones = new List<GameObject>();
    [SerializeField] private float _minTimeStoneSpawn;
    private float _lastTimeStoneSpawn;
    [SerializeField] private List<GameObject> _grass = new List<GameObject>();
    [SerializeField] private float _minTimeGrassSpawn;
    private float _lastTimeGrassObjSpawn;
    [SerializeField] private List<GameObject> _other = new List<GameObject>();
    [SerializeField] private float _minTimeOtherObjSpawn;
    private float _lastTimeOtherObjSpawn;


    [SerializeField] private Transform _cloudPointSpawn;
    [SerializeField] private List<GameObject> _cloud = new List<GameObject>();
    [SerializeField] private float _minTimeCloudSpawn;
    private float _lastTimeCloudSpawn;

    private List<BackGroundObject> _backGroundObjects = new List<BackGroundObject>();
    
    private void Start()
    {
        _backGroundObjects.Add(new BackGroundObject(_trees, _minTimeTreeSpawn));
        _backGroundObjects.Add(new BackGroundObject(_bush, _minTimeBushSpawn));
        _backGroundObjects.Add(new BackGroundObject(_stones, _minTimeStoneSpawn));
        _backGroundObjects.Add(new BackGroundObject(_grass, _minTimeGrassSpawn));
        _backGroundObjects.Add(new BackGroundObject(_other, _minTimeOtherObjSpawn));
    }

    private void Update()
    {
        foreach (var groundObject in _backGroundObjects)
        {
            groundObject._lastTimeSpawn += Time.deltaTime;
            if (groundObject._lastTimeSpawn >= groundObject._minTimeSpawn)
            {
                if (Random.Range(0, 10) == 0) 
                    groundObject._lastTimeSpawn = groundObject._minTimeSpawn/3;
                else
                    groundObject._lastTimeSpawn = Random.Range(-groundObject._minTimeSpawn/2, 0);
                
                SpawnObject(groundObject);
                if (groundObject._minTimeSpawn != _minTimeGrassSpawn) // Если заспавненный объект не трава
                {
                    foreach (var otherObject in _backGroundObjects) // после спавна предмета кого-то сделаем, что бы все 
                    {                      // следующие заспавнились чуть позже, что бы не было возможного нагромождения
                        if (groundObject != otherObject)
                        {
                            if (otherObject._minTimeSpawn - otherObject._minTimeSpawn < 0.25f)
                                otherObject._lastTimeSpawn -= 0.25f;
                        }
                    }
                }
            }
        }

        _lastTimeCloudSpawn += Time.deltaTime;
        if (_lastTimeCloudSpawn >= _minTimeCloudSpawn)
        {
            Instantiate(_cloud[Random.Range(0, _cloud.Count)],
                 _cloudPointSpawn.position + new Vector3(0, Random.Range(0f, 1.4f)), Quaternion.identity);
            if (Random.Range(0, 4) == 0) 
                _lastTimeCloudSpawn = _minTimeCloudSpawn/3;
            else
                _lastTimeCloudSpawn = Random.Range(-_lastTimeCloudSpawn/2, 0);
        }
    }

    private void SpawnObject(BackGroundObject groundObject)
    {
        GameObject randObj = groundObject._objects[Random.Range(0, groundObject._objects.Count)]; 
        // выбираем случайный объект из перечня и немного его редактируем случайным образом 
        
        GameObject spawnedObj = Instantiate(randObj, _spawnPoint.position, Quaternion.identity);
        spawnedObj.AddComponent<CircleCollider2D>();
        SpriteRenderer renderer = spawnedObj.GetComponent<SpriteRenderer>();

        Vector3 localScale = spawnedObj.transform.localScale;
        localScale = new Vector2(Random.Range(localScale.x * 0.92f, localScale.x * 1.08f), 
             Random.Range(localScale.y * 0.92f, localScale.y * 1.08f));
        spawnedObj.transform.localScale = localScale;

        // на сцене буду объекты которые будут спавнить перед игроком (его sortingOrder = 0), их sortingOrder = 1
        // и за игроком, их sortingOrder = -2 и они будут немного темнее, что бы показать, что они позади
        if (Random.Range(0, 2) == 0)
        {
            renderer.sortingOrder = -2;
            Color color = renderer.color;
            color.r /= 1.08f;
            color.g /= 1.08f;
            color.b /= 1.08f;
            renderer.color = color;
            if (renderer.gameObject.transform.childCount != 0) 
            {
                for (int i = 0; i < renderer.gameObject.transform.childCount; i++)
                {
                    SpriteRenderer childRenderer = renderer.gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>();
                    childRenderer.color = color;
                    childRenderer.sortingOrder = -1;
                }
            }
        }
        else
        {
            renderer.sortingOrder = 8;
            if (renderer.gameObject.transform.childCount != 0)
                for (int i = 0; i < renderer.gameObject.transform.childCount; i++)
                {
                    SpriteRenderer childRenderer = renderer.gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>();
                    childRenderer.sortingOrder = 9;
                }
        }
        if (Random.Range(0, 2) == 0)
            renderer.flipX = !renderer.flipX;
    }

    private class BackGroundObject
    {
        internal List<GameObject> _objects; // тип предметов (деревья, камни, кусты...)
        internal float _minTimeSpawn; // минимальное время между их спавнами (максимальное в полтора раза больше )
        internal float _lastTimeSpawn; // ведет отсчет 

        internal BackGroundObject(List<GameObject> objects, float timeSpawn)
        {
            _objects = objects;
            _minTimeSpawn = timeSpawn;
            _lastTimeSpawn = Random.Range(0, timeSpawn);
        }
    }
}
