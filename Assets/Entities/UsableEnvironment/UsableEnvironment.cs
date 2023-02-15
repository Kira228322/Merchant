using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UsableEnvironment : ClickableIfPlayerNear
{
    [SerializeField] private string _warningLabel;
    [SerializeField] private string _warningMessage;
    [SerializeField] private Sprite _spriteAfterUse; // Sprite �� ������� ����� ������ ������� �������
    [SerializeField] private ParticleSystem _particleSystem; // �������� ����� ��� ����������� 
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if(IsFunctionalComplete())
        {
            GetComponent<SpriteRenderer>().sprite = _spriteAfterUse;
            _particleSystem.Stop();
        }
        else
            CanvasWarningGenerator.Instance.CreateWarning(_warningLabel, _warningMessage);
    }

    protected abstract bool IsFunctionalComplete(); // ���������� true ���� ��� ������ ��� ����
}
