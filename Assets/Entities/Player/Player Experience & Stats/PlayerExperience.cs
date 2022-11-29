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
        if (level <= -1) return 0; //Если игрок 0 уровня и хотим узнать, сколько нужно было опыта, чтобы получить 0 уровень. Сейчас это используется в ЭКСП - баре
        if (level <= 30)
        {
            //y = 50 + 10x + 2* x^2 - 0.1 * (x - (2x)^(1/2) - 1)^3
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
            AddExperience(0); //Если получил опыта сразу на 2 и более уровня
        }
        ExperienceChanged?.Invoke();
    }
}
