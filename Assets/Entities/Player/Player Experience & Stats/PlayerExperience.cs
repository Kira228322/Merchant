using UnityEngine;
using UnityEngine.Events;
public class PlayerExperience
{
    public int CurrentExperience { get; private set; }
    public int CurrentLevel { get; private set; }
    public int UnspentSkillPoints { get; set; }

    public event UnityAction LevelUp;
    public event UnityAction ExperienceChanged;

    public PlayerExperience()
    {
        CurrentLevel = 0;
        CurrentExperience = 0;
        UnspentSkillPoints = 0;
    }

    public int ExperienceNeededForNextLevel()
    {
        int result = 0;
        for (int i = 0; i <= CurrentLevel; i++)
        {
            result += ExperienceNeededForLevel(i);
        }
        return result;
    }
    private int ExperienceNeededForLevel(int level)
    {
        if (level <= 30)
        {
            return (int)Mathf.Ceil((float)(50 + 10 * level + 2 * level * level - 0.1 * Mathf.Pow(level - Mathf.Sqrt(2 * level) - 1, 3)));
        }
        else return 1200;
    }

    public void IncreaseLevel()
    {
        CurrentLevel++;
        UnspentSkillPoints++;
        LevelUp?.Invoke();
    }

    public void AddExperience (int amount) 
    {
        CurrentExperience += amount;
        if (CurrentExperience >= ExperienceNeededForNextLevel())
        {
            IncreaseLevel();
            AddExperience(0); //≈сли получил опыта сразу на 2 и более уровн€
        }
        ExperienceChanged?.Invoke();
    }
}
