using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftingStation : MonoBehaviour, IPointerClickHandler
{
    private float _distanceToUse = 3.1f;
    [SerializeField] private CraftingStationSO _craftingStationSO;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if ((transform.position - Player.Instance.transform.position).magnitude > _distanceToUse)
            return;
        FindObjectOfType<CraftingHandler>(true).SetCraftingStation(_craftingStationSO.Icon, _craftingStationSO.Text, _craftingStationSO.type);
    }
}
