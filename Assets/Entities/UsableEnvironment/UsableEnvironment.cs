using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UsableEnvironment : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string _warningLabel;
    [SerializeField] private string _warningMessage;
    [SerializeField] private Sprite _spriteAfterUse; // Sprite на который будет носить юзнутый предмет
    [SerializeField] private ParticleSystem _particleSystem; // партиклы после юза выключаются 

    private float _distanceToUse = 3;
    public void OnPointerClick(PointerEventData eventData)
    {
        if ((transform.position - Player.Instance.transform.position).magnitude > _distanceToUse)
            return;
        
        if(IsFunctionalComplete())
        {
            GetComponent<SpriteRenderer>().sprite = _spriteAfterUse;
            _particleSystem.Stop();
        }
        else
            CanvasWarningGenerator.Instance.CreateWarning(_warningLabel, _warningMessage);
    }

    protected abstract bool IsFunctionalComplete(); // возвращает true если все прошло как надо

}
