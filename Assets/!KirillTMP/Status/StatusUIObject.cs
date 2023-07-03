using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUIObject : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _shell;
    [SerializeField] private GameObject _infoWindow;
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
    
    public void Init(Status status, bool lowNeedsDebuff)
    {
        _status = status;

        _currentHourDuration = status.HourDuration;
        _icon.sprite = status.Icon;
        _shell.color = new Color(255f/255, 24f/255, 0);
        GameTime.MinuteChanged += CheckPlayerNeedForLowNeedsDebuff; 
    }

    private void CheckPlayerNeedForLowNeedsDebuff() //ля офигенное название, можно было короче, но так понятнее
    {
        if (Player.Instance.Needs.CurrentSleep > 0 && Player.Instance.Needs.CurrentHunger > 0)
        {
            GameTime.MinuteChanged -= CheckPlayerNeedForLowNeedsDebuff; 
            Destroy(gameObject);
        }
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

    public void OnClick()
    {
        GameObject window = Instantiate(_infoWindow, gameObject.transform);
        window.GetComponent<StatusInfoWindow>().Init
            (_status.StatusName, _status.StatusDescription, _currentHourDuration, _status.HourDuration);
        
        Vector3 position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        position += new Vector3(0, _icon.rectTransform.rect.height + window.GetComponent<Image>().rectTransform.rect.height / 2, 0);
        
        window.transform.position = position;
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
                    Player.Instance.Statistics.AdditionalLuck += effect.value;
                    break;
                case Status.Effect.Stat.ExpGain:
                    Player.Instance.Experience.ExpGain += effect.value/100f;
                    break;
                case Status.Effect.Stat.Diplomacy:
                    Player.Instance.Statistics.AdditionalDiplomacy += effect.value;
                    break;
                case Status.Effect.Stat.Toughness:
                    Player.Instance.Statistics.AdditionalToughness += effect.value;
                    break;
                case Status.Effect.Stat.MoveSpeed:
                    Player.Instance.PlayerMover.SpeedModifier += effect.value/100f;
                    break;
                // TODO
            }
        }

    }
    
    private void Deactivation()
    {
        foreach (var effect in _status.Effects)
        {
            switch (effect.stat)
            {
                case Status.Effect.Stat.Luck:
                    Player.Instance.Statistics.AdditionalLuck -= effect.value;
                    break;
                case Status.Effect.Stat.ExpGain:
                    Player.Instance.Experience.ExpGain -= effect.value/100f;
                    break;
                case Status.Effect.Stat.Diplomacy:
                    Player.Instance.Statistics.AdditionalDiplomacy -= effect.value;
                    break;
                case Status.Effect.Stat.Toughness:
                    Player.Instance.Statistics.AdditionalToughness -= effect.value;
                    break;
                case Status.Effect.Stat.MoveSpeed:
                    Player.Instance.PlayerMover.SpeedModifier -= effect.value/100f;
                    break;
                // TODO
            }
        }
    }
}
