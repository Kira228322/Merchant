using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomBGGenerator : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private List<GameObject> _trees = new List<GameObject>();
    [SerializeField] private float _minTimeTreeSpawn;
    private float _lastTimeTreeSpawn;
    [SerializeField] private List<GameObject> _brushes = new List<GameObject>();
    [SerializeField] private float _minTimeBrushSpawn;
    private float _lastTimeBrushSpawn;
    [SerializeField] private List<GameObject> _stones = new List<GameObject>();
    [SerializeField] private float _minTimeStoneSpawn;
    private float _lastTimeStoneSpawn;
    [SerializeField] private List<GameObject> _other = new List<GameObject>();
    [SerializeField] private float _minTimeOtherObjSpawn;
    private float _lastTimeOtherObjSpawn;

    private List<BackGroundObject> _backGroundObjects = new List<BackGroundObject>();
    private void Start()
    {
        _backGroundObjects.Add(new BackGroundObject(_trees, _minTimeTreeSpawn));
        _backGroundObjects.Add(new BackGroundObject(_brushes, _minTimeBrushSpawn));
        _backGroundObjects.Add(new BackGroundObject(_stones, _minTimeStoneSpawn));
        _backGroundObjects.Add(new BackGroundObject(_other, _minTimeOtherObjSpawn));
    }

    private void Update()
    {
        foreach (var groundObject in _backGroundObjects)
        {
            groundObject._lastTimeSpawn += Time.deltaTime;
            if (groundObject._lastTimeSpawn >= groundObject._minTimeSpawn)
            {
                groundObject._lastTimeSpawn = Random.Range(-groundObject._minTimeSpawn, 0);
                // Spawn
            }
        }
    }

    private void SpawnObject(BackGroundObject groundObject)
    {
        GameObject spawnedObj = groundObject._objects[Random.Range(0, groundObject._objects.Count)];
        SpriteRenderer renderer = spawnedObj.GetComponent<SpriteRenderer>();
        renderer.size = new Vector2(Random.Range(renderer.size.x * 0.9f, renderer.size.x * 1.1f), 
            Random.Range(renderer.size.y * 0.9f, renderer.size.y * 1.1f));
        Instantiate(spawnedObj, _spawnPoint);
    }

    private class BackGroundObject
    {
        internal List<GameObject> _objects;
        internal float _minTimeSpawn;
        internal float _lastTimeSpawn;

        internal BackGroundObject(List<GameObject> objects, float timeSpawn)
        {
            _objects = objects;
            _minTimeSpawn = timeSpawn;
            _lastTimeSpawn = Random.Range(0, timeSpawn);
        }
    }
}
