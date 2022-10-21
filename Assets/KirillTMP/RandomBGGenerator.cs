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
    [SerializeField] private List<GameObject> _grass = new List<GameObject>();
    [SerializeField] private float _minTimeGrassSpawn;
    private float _lastTimeGrassObjSpawn;
    [SerializeField] private List<GameObject> _other = new List<GameObject>();
    [SerializeField] private float _minTimeOtherObjSpawn;
    private float _lastTimeOtherObjSpawn;

    private List<BackGroundObject> _backGroundObjects = new List<BackGroundObject>();
    private void Start()
    {
        _backGroundObjects.Add(new BackGroundObject(_trees, _minTimeTreeSpawn));
        _backGroundObjects.Add(new BackGroundObject(_brushes, _minTimeBrushSpawn));
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
                groundObject._lastTimeSpawn = Random.Range(-groundObject._minTimeSpawn/2, 0);
                SpawnObject(groundObject);
                if (groundObject._minTimeSpawn != _minTimeGrassSpawn) // ���� ������������ ������ �� �����
                {
                    foreach (var otherObject in _backGroundObjects) // ����� ������ �������� ����-�� �������, ��� �� ��� 
                    {                      // ��������� ������������ ���� �����, ��� �� �� ���� ���������� �������������
                        if (groundObject != otherObject)
                        {
                            if (otherObject._minTimeSpawn - otherObject._minTimeSpawn < 0.25f)
                                otherObject._lastTimeSpawn -= 0.25f;
                        }
                    }
                }
            }
        }
    }

    private void SpawnObject(BackGroundObject groundObject)
    {
        GameObject randObj = groundObject._objects[Random.Range(0, groundObject._objects.Count)]; 
        // �������� ��������� ������ �� ������� � ������� ��� ����������� ��������� ������� 
        
        GameObject spawnedObj = Instantiate(randObj, _spawnPoint.position, Quaternion.identity);
        
        SpriteRenderer renderer = spawnedObj.GetComponent<SpriteRenderer>();

        Vector3 localScale = spawnedObj.transform.localScale;
        localScale = new Vector2(Random.Range(localScale.x * 0.92f, localScale.x * 1.08f), 
             Random.Range(localScale.y * 0.92f, localScale.y * 1.08f));
        spawnedObj.transform.localScale = localScale;

        // �� ����� ���� ������� ������� ����� �������� ����� ������� (��� sortingOrder = 0), �� sortingOrder = 1
        // � �� �������, �� sortingOrder = -1 � ��� ����� ������� ������, ��� �� ��������, ��� ��� ������
        if (Random.Range(0, 2) == 0)
        {
            renderer.sortingOrder = -1;
            Color color = renderer.color;
            color.r /= 1.1f;
            color.g /= 1.1f;
            color.b /= 1.1f;
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
            renderer.sortingOrder = 1;
            if (renderer.gameObject.transform.childCount != 0)
                for (int i = 0; i < renderer.gameObject.transform.childCount; i++)
                {
                    SpriteRenderer childRenderer = renderer.gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>();
                    childRenderer.sortingOrder = 1;
                }
        }
        if (Random.Range(0, 2) == 0)
            renderer.flipX = !renderer.flipX;
    }

    private class BackGroundObject
    {
        internal List<GameObject> _objects; // ��� ��������� (�������, �����, �����...)
        internal float _minTimeSpawn; // ����������� ����� ����� �� �������� (������������ � ������� ���� ������ )
        internal float _lastTimeSpawn; // ����� ������ 

        internal BackGroundObject(List<GameObject> objects, float timeSpawn)
        {
            _objects = objects;
            _minTimeSpawn = timeSpawn;
            _lastTimeSpawn = Random.Range(0, timeSpawn);
        }
    }
}
