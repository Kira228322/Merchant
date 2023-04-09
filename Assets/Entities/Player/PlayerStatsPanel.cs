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
    [SerializeField] private TMP_Text _craftingText;

    private List<TMP_Text> _statsList = new();

    private Player _player;
    private PlayerStats _playerStats;
    private PlayerExperience _playerExperience;
    private void Start()
    {
        _player = Player.Instance;
        _playerStats = _player.Statistics;
        _playerExperience = _player.Experience;
        _playerExperience.ExperienceChanged += Refresh;

        _statsList.Add(_diplomacyText);
        _statsList.Add(_luckText);
        _statsList.Add(_toughnessText);
        _statsList.Add(_craftingText);
        Refresh();
    }
    private void OnDisable()
    {
        _playerExperience.ExperienceChanged -= Refresh;
    }

    private void Refresh()
    {
        _currentLevelText.text = "������� �������: " + _playerExperience.CurrentLevel;

         _experienceBarExperienceText.text = $"{_playerExperience.CurrentExperience - _playerExperience.ExperienceNeededForAllLevelsBefore(_playerExperience.CurrentLevel - 1)} / {_playerExperience.ExperienceNeededForLevel(_playerExperience.CurrentLevel)}";

        _experienceBarSlider.value = (float)(_playerExperience.CurrentExperience - //���������� �� ���������, ��� �����
            _playerExperience.ExperienceNeededForAllLevelsBefore(_playerExperience.CurrentLevel - 1))
            / (float)_playerExperience.ExperienceNeededForLevel(_playerExperience.CurrentLevel);
        
        _unspentSkillPointsText.text = "��������������� �����: " + _playerExperience.UnspentSkillPoints;
        if (_playerExperience.UnspentSkillPoints > 0) //���������� ������ "+" ����� ������� �����, ����� ��������, ��� �� ����� ���������
        {
            foreach (TMP_Text statText in _statsList)
            {
                statText.GetComponentInChildren<Button>().interactable = true; //���������������, ��� ���� �� ������ ��� �����, � � ���� ���� ��� ������.
            }
        }
        else
        {
            foreach (TMP_Text statText in _statsList)
            {
                statText.GetComponentInChildren<Button>().interactable = false;
            }
        }

        _diplomacyText.text = "����������: " + _playerStats.TotalDiplomacy;
        _luckText.text = "�����: " + _playerStats.TotalLuck;
        _toughnessText.text = "���������: " + _playerStats.TotalToughness;
        _craftingText.text = "��������: " + _playerStats.TotalCrafting;

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
    public void LevelUpCrafting()
    {
        _playerStats.IncreaseCrafting();
        OnStatLvlUpButtonPressed();
    }
    private void OnStatLvlUpButtonPressed()
    {
        _playerExperience.UnspentSkillPoints--;
        Refresh();
    }

}
