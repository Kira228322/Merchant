using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cloud : MonoBehaviour
{
    private float _speed; // облака должны двигаться так же медленно, как бекграунд, поэтому они будут двигаться
                          // против движения 

    private void Start()
    {
        // Так же их вид рандомизируется, как и другие объекты
        
        _speed = Random.Range(0.6f, 1.2f);
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = Random.Range(color.a * 0.8f, color.a);
        GetComponent<SpriteRenderer>().color = color;
        Vector3 localScale = transform.localScale;
        localScale = new Vector2(Random.Range(localScale.x * 0.8f, localScale.x * 1.2f), 
            Random.Range(localScale.y * 0.8f, localScale.y * 1.2f));
        transform.localScale = localScale;
    }

    void Update()
    {
        transform.position += Vector3.right * _speed * Time.deltaTime;
    }
}
