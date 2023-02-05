using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    public Wheel Wheel;
    public Body Body;
    public Suspension Suspension;

    [SerializeField] private SpriteRenderer _wheelSprite;
    [SerializeField] private SpriteRenderer _bodySprite;
    [SerializeField] private SpriteRenderer _suspensionSprite;
    
    private float _qualityModifier; // ��� ���������� �������� ������ ����� ������� ������������ ���� �������� 
    
    
    public void RefreshStats()
    {
        _qualityModifier = Wheel.QualityModifier;

        int rowsToAdd = Body.InventoryRows - Player.Singleton.Inventory.ItemGrid.GridSizeHeight;
        Player.Singleton.Inventory.ItemGrid.AddRowsToInventory(rowsToAdd);

        //��� ����� ��������������� � �������

        _wheelSprite.sprite = Wheel.Sprite;
        _bodySprite.sprite = Body.Sprite;
        _suspensionSprite.sprite = Suspension.Sprite;
    }
}
