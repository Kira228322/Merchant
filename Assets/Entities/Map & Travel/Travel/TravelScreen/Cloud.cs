using UnityEngine;
using Random = UnityEngine.Random;

public class Cloud : MonoBehaviour
{
    private float _speed; // облака должны двигаться так же медленно, как бекграунд, поэтому они будут двигаться
                          // против движения 

    private void Start()
    {
        // Так же их вид рандомизируется, как и другие объекты

        _speed = Random.Range(0.7f, 1.4f);
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = Random.Range(color.a * 0.7f, color.a * 0.99f);
        GetComponent<SpriteRenderer>().color = color;
        Vector3 localScale = transform.localScale;
        localScale = new Vector2(Random.Range(localScale.x * 0.8f, localScale.x * 1.2f),
            Random.Range(localScale.y * 0.8f, localScale.y * 1.2f));
        transform.localScale = localScale;
    }

    void Update()
    {
        transform.position += _speed * Time.deltaTime * Vector3.right;
    }
}
