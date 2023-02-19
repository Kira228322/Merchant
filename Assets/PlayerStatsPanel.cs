using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class PlayerStatsPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _currentLevelText;
    [SerializeField] private Slider _experienceBarSlider;
    [SerializeField] private TMP_Text _experienceBarExperienceText;

    [SerializeField] private TMP_Text _unspentSkillPointsText;

    [Header("Stats")]
    [SerializeField] private TMP_Text _diplomacyText;
    [SerializeField] private TMP_Text _luckText;
    [SerializeField] private TMP_Text _toughnessText;

    private List<TMP_Text> _statsList = new();

    private Player _player;
    private PlayerStats _playerStats;
    private PlayerExperience _playerExperience;
    private void Awake()
    {
        _player = Player.Instance;
        _playerStats = _player.Statistics;
        _playerExperience = _player.Experience;

        _statsList.Add(_diplomacyText);
        _statsList.Add(_luckText);
        _statsList.Add(_toughnessText);
    }
    private void OnEnable()
    {
        _playerExperience.ExperienceChanged += Refresh;
    }
    private void OnDisable()
    {
        _playerExperience.ExperienceChanged -= Refresh;
    }
    private void Start()
    {
        Refresh();
    }
    private void Refresh()
    {
        _currentLevelText.text = "Текущий уровень: " + _playerExperience.CurrentLevel;

        //_experienceBarExperienceText.text = $"{_playerExperience.CurrentExperience} / {_playerExperience.ExperienceNeededForNextLevel()}";
        // какой вариант лучше, сверху или снизу? Решили, что лучше снизу
         _experienceBarExperienceText.text = $"{_playerExperience.CurrentExperience - _playerExperience.ExperienceNeededForAllLevelsBefore(_playerExperience.CurrentLevel - 1)} / {_playerExperience.ExperienceNeededForLevel(_playerExperience.CurrentLevel)}";

        _experienceBarSlider.value = (float)(_playerExperience.CurrentExperience - //приведение не избыточно, иди нахуй
            _playerExperience.ExperienceNeededForAllLevelsBefore(_playerExperience.CurrentLevel - 1))
            / (float)_playerExperience.ExperienceNeededForLevel(_playerExperience.CurrentLevel);
        
        _unspentSkillPointsText.text = "Нераспределённых очков: " + _playerExperience.UnspentSkillPoints;
        if (_playerExperience.UnspentSkillPoints > 0) //Отобразите кнопки "+" возле каждого стата, чтобы показать, что их можно прокачать
        {
            foreach (TMP_Text statText in _statsList)
            {
                statText.GetComponentInChildren<Button>().interactable = true; //подразумевается, что поле со статой это текст, и у него есть дитё кнопка.
            }
        }
        else
        {
            foreach (TMP_Text statText in _statsList)
            {
                statText.GetComponentInChildren<Button>().interactable = false;
            }
        }

        _diplomacyText.text = "Дипломатия: " + _playerStats.Diplomacy;
        _luckText.text = "Удача: " + _playerStats.Luck;
        _toughnessText.text = "Стойкость: " + _playerStats.Toughness;

    }

    public void LevelUpDiplomacy()
    {
        _playerStats.IncreaseDiplomacy();
        OnStatLvlUpButtonPressed();
    }
    public void LevelUpLuck()
    {
        _playerStats.IncreaseLuck();
        OnStatLvlUpButtonPressed();
    }
    public void LevelUpToughness()
    {
        _playerStats.IncreaseToughness();
        OnStatLvlUpButtonPressed();
    }
    private void OnStatLvlUpButtonPressed()
    {
        _playerExperience.UnspentSkillPoints--;
        Refresh();
    }

}
