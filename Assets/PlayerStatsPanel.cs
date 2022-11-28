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
        _currentLevelText.text = "������� �������: " + _playerExperience.CurrentLevel;
        _currentExperienceText.text = "������� ���������� �����: " + _playerExperience.CurrentExperience;
        _experienceNeededForNextLevelText.text = "�������� ����� �� ������ ������:" +
            (_playerExperience.ExperienceNeededForNextLevel() - _playerExperience.CurrentExperience);

        _unspentSkillPointsText.text = "��������������� �����: " + _playerExperience.UnspentSkillPoints;
        if (_playerExperience.UnspentSkillPoints > 0) //���������� ������ "+" ����� ������� �����, ����� ��������, ��� �� ����� ���������
        {
            foreach (TMP_Text statText in _statsList)
            {
                statText.GetComponentInChildren<Button>().gameObject.SetActive(true); //���������������, ��� ���� �� ������ ��� �����, � � ���� ���� ��� ������.
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
        //�������� ���� �����??
        _playerExperience.UnspentSkillPoints--;
        Refresh();
    }

}
