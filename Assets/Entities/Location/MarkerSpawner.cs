using UnityEngine;

public class MarkerSpawner : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _usableObject;
    [SerializeField] private GameObject _markerPrefab;
    [SerializeField] private Transform _spawnPoint;
    private Marker _currentMarker;
    private BoxCollider2D _boxCollider2D;

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _currentMarker = Instantiate(_markerPrefab, _spawnPoint).GetComponent<Marker>();
        _currentMarker.Init(_usableObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Destroy(_currentMarker.gameObject);
    }

    public void DisableMarkerSpawner()
    {
        if (_currentMarker != null)
            Destroy(_currentMarker.gameObject);
        _boxCollider2D.enabled = false;
    }

    public void EnableMarkerSpawner()
    {
        _boxCollider2D.enabled = true;
    }
}
