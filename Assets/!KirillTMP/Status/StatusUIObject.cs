using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUIObject : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _shell;
    private Status _status;
    public Status Status => _status;
    private float _currentHourDuration;

    public void Init(Status status)
    {
        _status = status;

        _currentHourDuration = status.HourDuration;
        _icon.sprite = status.Icon;
        if (status.Type == Status.StatusType.Buff)
            _shell.color = new Color(39f/255, 255f/255, 0);
        else 
            _shell.color = new Color(255f/255, 24f/255, 0);
    }
    
    private void Start()
    {
        GameTime.MinuteChanged += DurationDecrease;
        Activation();
    }

    private void OnDisable()
    {
        GameTime.MinuteChanged -= DurationDecrease;
        Deactivation(); 
    }
    
    private void DurationDecrease()
    {
        _currentHourDuration -= 0.01667f; // 1/60
        _shell.fillAmount = _currentHourDuration / _status.HourDuration;
        if (_currentHourDuration <= 0)
            Destroy(gameObject);
    }

    public void RefreshStatus()
    {
        _currentHourDuration = _status.HourDuration;
    }
    
    private void Activation()
    {
        
        
        foreach (var effect in _status.Effects)
        {
            switch (effect.stat)
            {
                case Status.Effect.Stat.Luck:
                    Player.Instance.Statistics.AddititionalLuck += effect.value;
                    break;
                // TODO
            }
        }
        
        Player.Instance.Statistics.RefreshStats();
    }
    
    private void Deactivation()
    {
        foreach (var effect in _status.Effects)
        {
            switch (effect.stat)
            {
                case Status.Effect.Stat.Luck:
                    Player.Instance.Statistics.AddititionalLuck -= effect.value;
                    break;
                // TODO
            }
        }
        
        Player.Instance.Statistics.RefreshStats();
    }
}
