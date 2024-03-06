using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StartDecorateObject : MonoBehaviour
{
    [SerializeField] private List<Sprite> _possibleSprite;
    private void Start()
    {
        if (Random.Range(0, 2) == 0)
            Destroy(gameObject);
        else
            GetComponent<SpriteRenderer>().sprite = _possibleSprite[Random.Range(0, _possibleSprite.Count)];

    }
}
