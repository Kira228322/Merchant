using System;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class PlayerExperience
{
    #region ѕол€, свойства и событи€

    [SerializeField, HideInInspector] private int _currentExperience;
    [SerializeField, HideInInspector] private int _currentLevel;
    [SerializeField, HideInInspector] private int _unspentSkillPoints;
    [SerializeField, HideInInspector] public float ExpGain;

    public int CurrentExperience => _currentExperience;
    public int CurrentLevel => _currentLevel;
    public int UnspentSkillPoints
    {
        get
        {
            return _unspentSkillPoints;
        }
        set
        {
            _unspentSkillPoints = value;
            if (_unspentSkillPoints < 0)
            {
                _unspentSkillPoints = 0;
            }
            SkillPointsChanged?.Invoke();
        }
    }


    public event UnityAction LevelUp;
    public event UnityAction ExperienceChanged;
    public event UnityAction SkillPointsChanged;
    #endregion
    public int ExperienceNeededForNextLevel()
    {
        return ExperienceNeededForAllLevelsBefore(CurrentLevel);
    }
    public int ExperienceNeededForAllLevelsBefore(int level)
    {
        int result = 0;
        for (int i = 0; i <= level; i++)
        {
            result += ExperienceNeededForLevel(i);
        }
        return result;
    }
    public int ExperienceNeededForLevel(int level)
    {
        if (level <= -1) return 0;
        if (level <= 30)
        {
            //y = 50 + 10x + 2 * x^2 - 0.1 * (x - (2x)^(1/2) - 1)^3
            return (int)Mathf.Ceil((float)(50 + 10 * level + 2 * level * level - 0.1 * Mathf.Pow(level - Mathf.Sqrt(2 * level) - 1, 3)));
        }
        else return 1200;
    }

    public void IncreaseLevel()
    {
        _currentLevel++;
        UnspentSkillPoints++;
        LevelUp?.Invoke();
    }

    public void AddExperience (int amount) 
    {
        _currentExperience += Convert.ToInt32(amount * (1 + ExpGain));
        if (CurrentExperience >= ExperienceNeededForNextLevel())
        {
            IncreaseLevel();
            AddExperience(0); //≈сли получил опыта сразу на 2 и более уровн€. Ёто конечно глупа€ рекурси€ и может даже считатьс€ костылЄм, но будет работать
        }
        ExperienceChanged?.Invoke();
    }

    public bool AnyUnspentSkillPoints()
    {
        return UnspentSkillPoints > 0;
    }
}
