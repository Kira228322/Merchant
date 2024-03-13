using UnityEngine;
using UnityEngine.EventSystems;

public class Ground : MonoBehaviour, IPointerClickHandler
{
    private PlayerMover _playerMover;

    private void Start()
    {
        _playerMover = FindObjectOfType<PlayerMover>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 targetPosition = Camera.main.ScreenPointToRay(eventData.position).origin;
        targetPosition = new Vector3(targetPosition.x, _playerMover.transform.position.y);

        _playerMover.StartMove(_playerMover.transform.position, targetPosition);
    }


}
