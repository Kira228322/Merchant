using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WagonClickFunctional : MonoBehaviour, IPointerClickHandler
{
    
    [SerializeField] private GameObject _functionalWindow;
    private float _distanceToUse = 3.5f;
    
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if ((transform.position - Player.Instance.transform.position).magnitude > _distanceToUse)
            return;
        if (GameManager.Instance.CurrentFunctionalWindow != null)
            return;
        
        GameObject window = Instantiate(_functionalWindow, MapManager.Canvas.transform);
        window.transform.position = Camera.main.WorldToScreenPoint(new Vector3((transform.position.x + Player.Instance.transform.position.x) / 2,
            transform.position.y + 3.5f));
    }
    
}
