using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class PlayerStatsPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _currentLevelText;
    [SerializeField] private TMP_Text _currentExperienceText;
    [SerializeField] private TMP_Text _experienceNeededForNextLevelText;

    [SerializeField] private TMP_Text _unspentSkillPointsText;

    [SerializeField] private List<TMP_Text> _statsList = new List<TMP_Text>();

    private Player _player;
    private PlayerStats _playerStats;
    private PlayerExperience _playerExperience;
    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _playerStats = _player.Statistics;
        _playerExperience = _player.Experience;
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
        _currentExperienceText.text = "Текущее количество опыта: " + _playerExperience.CurrentExperience;
        _experienceNeededForNextLevelText.text = "Осталось опыта до нового уровня:" +
            (_playerExperience.ExperienceNeededForNextLevel() - _playerExperience.CurrentExperience);

        _unspentSkillPointsText.text = "Нераспределённых очков: " + _playerExperience.UnspentSkillPoints;
        if (_playerExperience.UnspentSkillPoints > 0) //Отобразите кнопки "+" возле каждого стата, чтобы показать, что их можно прокачать
        {
            foreach (TMP_Text statText in _statsList)
            {
                statText.GetComponentInChildren<Button>().gameObject.SetActive(true); //подразумевается, что поле со статой это текст, и у него есть дитё кнопка.
            }
        }
        else
        {
            foreach (TMP_Text statText in _statsList)
            {
                statText.GetComponentInChildren<Button>().gameObject.SetActive(false);
            }
        }

        foreach (TMP_Text statText in _statsList)
        {
           // statText.GetComponentInChildren<TMP_Text>().text;
        }

    }

    public void OnStatLvlUpButtonPressed()
    {
        //получить саму стату??
        _playerExperience.UnspentSkillPoints--;
        Refresh();
    }

}
