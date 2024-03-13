using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomBGGenerator : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private List<GameObject> _trees = new();
    [SerializeField] private float _minTimeTreeSpawn;
    [SerializeField] private List<GameObject> _bush = new();
    [SerializeField] private float _minTimeBushSpawn;
    [SerializeField] private List<GameObject> _stones = new();
    [SerializeField] private float _minTimeStoneSpawn;
    [SerializeField] private List<GameObject> _grass = new();
    [SerializeField] private float _minTimeGrassSpawn;
    [SerializeField] private List<GameObject> _other = new();
    [SerializeField] private float _minTimeOtherObjSpawn;
    [SerializeField] private List<GameObject> _roadLamps = new();
    [SerializeField] private float _minTimeRoadLampsObjSpawn;


    [SerializeField] private Transform _cloudPointSpawn;
    [SerializeField] private List<GameObject> _cloud = new();
    [SerializeField] private float _minTimeCloudSpawn;
    private float _lastTimeCloudSpawn;

    private List<BackGroundObject> _backGroundObjects = new();
    public List<Cloud> CloudsOnScene = new();
    private void Start()
    {
        _backGroundObjects.Add(new BackGroundObject(_trees, _minTimeTreeSpawn));
        _backGroundObjects.Add(new BackGroundObject(_bush, _minTimeBushSpawn));
        _backGroundObjects.Add(new BackGroundObject(_stones, _minTimeStoneSpawn));
        _backGroundObjects.Add(new BackGroundObject(_grass, _minTimeGrassSpawn));
        _backGroundObjects.Add(new BackGroundObject(_other, _minTimeOtherObjSpawn));
        _backGroundObjects.Add(new BackGroundObject(_roadLamps, _minTimeRoadLampsObjSpawn));
    }

    private void Update()
    {
        foreach (var groundObject in _backGroundObjects)
        {
            groundObject._lastTimeSpawn += Time.deltaTime;
            if (groundObject._lastTimeSpawn >= groundObject._minTimeSpawn)
            {
                if (Random.Range(0, 10) == 0)
                    groundObject._lastTimeSpawn = groundObject._minTimeSpawn / 3;
                else
                    groundObject._lastTimeSpawn = Random.Range(-groundObject._minTimeSpawn / 2, 0);

                SpawnObject(groundObject);
                if (groundObject._minTimeSpawn != _minTimeGrassSpawn) // Если заспавненный объект не трава
                {
                    foreach (var otherObject in _backGroundObjects) // после спавна предмета кого-то сделаем, что бы все 
                    {                      // следующие заспавнились чуть позже, что бы не было возможного нагромождения
                        if (groundObject != otherObject)
                        {
                            if (otherObject._minTimeSpawn - otherObject._lastTimeSpawn < 0.25f)
                                otherObject._lastTimeSpawn -= 0.25f;
                        }
                    }
                }
            }
        }

        _lastTimeCloudSpawn += Time.deltaTime;
        if (_lastTimeCloudSpawn >= _minTimeCloudSpawn)
        {
            CloudsOnScene.Add(Instantiate(_cloud[Random.Range(0, _cloud.Count)],
                 _cloudPointSpawn.position + new Vector3(0, Random.Range(0f, 1.4f)), Quaternion.identity)
                .GetComponent<Cloud>());
            if (Random.Range(0, 4) == 0)
                _lastTimeCloudSpawn = _minTimeCloudSpawn / 3;
            else
                _lastTimeCloudSpawn = Random.Range(-_lastTimeCloudSpawn / 2, 0);
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
        localScale = new Vector2(Random.Range(localScale.x * 0.93f, localScale.x * 1.08f),
             Random.Range(localScale.y * 0.93f, localScale.y * 1.08f));
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
                    if (renderer.gameObject.transform.GetChild(i).TryGetComponent(out SpriteRenderer childRenderer))
                    {
                        childRenderer.color = color;
                        childRenderer.sortingOrder = -1;
                    }
                }
            }
        }
        else
        {
            spawnedObj.transform.position += new Vector3(0, -0.1f); // объекты что ближе, ниже (так выглядит лучше)
            renderer.sortingOrder = 8;
            if (renderer.gameObject.transform.childCount != 0)
                for (int i = 0; i < renderer.gameObject.transform.childCount; i++)
                {
                    if (renderer.gameObject.transform.GetChild(i).TryGetComponent(out SpriteRenderer _))
                    {
                        SpriteRenderer childRenderer = renderer.gameObject.transform.GetChild(i).GetComponent<SpriteRenderer>();
                        childRenderer.sortingOrder = 9;
                    }
                }
        }
        if (Random.Range(0, 2) == 0)
            renderer.gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
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
            _lastTimeSpawn = Random.Range(timeSpawn * 0.6f, timeSpawn);
        }
    }
}
