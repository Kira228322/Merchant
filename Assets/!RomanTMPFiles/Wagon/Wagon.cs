using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    [HideInInspector] public Wheel Wheel;
    [HideInInspector] public Body Body;
    [HideInInspector] public Suspension Suspension;

    [SerializeField] private SpriteRenderer _wheelSprite;
    [SerializeField] private SpriteRenderer _bodySprite;
    [SerializeField] private SpriteRenderer _suspensionSprite;
    
    private float _qualityModifier; // ��� ���������� �������� ������ ����� ������� ������������ ���� �������� 
    
    
    public void RefreshStats()
    {
        // TODO
        // ��������, ����� ���������� ��������� ������ 
        
        // �������� ������ ���������, ���������� ���
        _qualityModifier = Wheel.QualityModifier;

        _wheelSprite.sprite = Wheel.Sprite;
        _bodySprite.sprite = Body.Sprite;
        _suspensionSprite.sprite = Suspension.Sprite;
    }
}
