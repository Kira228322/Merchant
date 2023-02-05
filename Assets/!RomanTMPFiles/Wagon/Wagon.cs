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
    
    private float _qualityModifier; // при вычислении качества дороги после поездки использовать этот параметр 
    
    
    public void RefreshStats()
    {
        // TODO
        // Вызывать, когда происходит улучшение телеги 
        
        // Изменить размер инвентаря, допустимый вес
        _qualityModifier = Wheel.QualityModifier;

        _wheelSprite.sprite = Wheel.Sprite;
        _bodySprite.sprite = Body.Sprite;
        _suspensionSprite.sprite = Suspension.Sprite;
    }
}
