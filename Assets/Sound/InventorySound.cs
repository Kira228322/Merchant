using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private ItemContainer _rightItemContainer;

    private void Start()
    {
        Player.Instance.Inventory.BaseItemGrid.ItemPlacedInTheGrid += PlaySound;
        Player.Instance.Inventory.QuestItemGrid.ItemPlacedInTheGrid += PlaySound;
        _rightItemContainer.ItemGrid.ItemPlacedInTheGrid += PlaySound;
        Player.Instance.Inventory.BaseItemGrid.ItemUpdated += PlaySound;
        Player.Instance.Inventory.QuestItemGrid.ItemUpdated += PlaySound;
        _rightItemContainer.ItemGrid.ItemUpdated += PlaySound;
    }

    private void OnDestroy()
    {
        Player.Instance.Inventory.BaseItemGrid.ItemPlacedInTheGrid -= PlaySound;
        Player.Instance.Inventory.QuestItemGrid.ItemPlacedInTheGrid -= PlaySound;
        _rightItemContainer.ItemGrid.ItemPlacedInTheGrid -= PlaySound;
        Player.Instance.Inventory.BaseItemGrid.ItemUpdated -= PlaySound;
        Player.Instance.Inventory.QuestItemGrid.ItemUpdated -= PlaySound;
        _rightItemContainer.ItemGrid.ItemUpdated -= PlaySound;
    }

    public void PlaySound(InventoryItem item)
    {
        _audioSource.PlayWithRandomPitch();
    }
    
    public void PlaySound(InventoryItem item, int amount)
    {
        if (amount > 0)
            _audioSource.PlayWithRandomPitch();
    }
}
