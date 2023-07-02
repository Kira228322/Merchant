using System;
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

    private bool isFirstEnable = true;
    
    [Header("Wagon")]
    [SerializeField] private Image _body;
    [SerializeField] private Image _suspension;
    [SerializeField] private Image _wheel;
    [SerializeField] private TMP_Text _slotsText;
    [SerializeField] private TMP_Text _weightText;
    [SerializeField] private TMP_Text _modifierText;
    private void Start()
    {
        isFirstEnable = false;
        _player = Player.Instance;
        _playerStats = _player.Statistics;
        _playerExperience = _player.Experience;


        _statsList.Add(_diplomacyText);
        _statsList.Add(_luckText);
        _statsList.Add(_toughnessText);
        _statsList.Add(_craftingText);
        Refresh();
    }
    private void OnEnable()
    {
        if (!isFirstEnable)
            Refresh();
        
        
        _body.sprite = Player.Instance.WagonStats.Body.Sprite;
        _suspension.sprite = Player.Instance.WagonStats.Suspension.Sprite;
        _wheel.sprite = Player.Instance.WagonStats.Wheel.Sprite;
        _slotsText.text = $"���������� ������: {Player.Instance.WagonStats.Body.InventoryRows * 5}";
        _weightText.text = $"����. ����� �����: {Player.Instance.WagonStats.Suspension.MaxWeight}��";
        _modifierText.text = $"����������� ������: {Math.Round(Player.Instance.WagonStats.Wheel.QualityModifier - 1, 2) * 100}";

        Player.Instance.Experience.ExperienceChanged += Refresh;
    }
    private void OnDisable()
    {
        Player.Instance.Experience.ExperienceChanged -= Refresh;
    }
    private void Refresh()
    {
        _currentLevelText.text = "������� �������: " + _playerExperience.CurrentLevel;

         _experienceBarExperienceText.text = $"{_playerExperience.CurrentExperience - _playerExperience.ExperienceNeededForAllLevelsBefore(_playerExperience.CurrentLevel - 1)} / {_playerExperience.ExperienceNeededForLevel(_playerExperience.CurrentLevel)}";

        _experienceBarSlider.value = (_playerExperience.CurrentExperience - 
            _playerExperience.ExperienceNeededForAllLevelsBefore(_playerExperience.CurrentLevel - 1))
            / (float)_playerExperience.ExperienceNeededForLevel(_playerExperience.CurrentLevel);
        
        _unspentSkillPointsText.text = "��������������� �����: " + _playerExperience.UnspentSkillPoints;
        if (_playerExperience.UnspentSkillPoints > 0) 
        {
            _unspentSkillPointsText.gameObject.SetActive(true);
            //���������� ������ "+" ����� ������� �����, ����� ��������, ��� �� ����� ���������
            foreach (TMP_Text statText in _statsList)
            {
                statText.transform.GetChild(0).gameObject.SetActive(true);
                //���������������, ��� ���� �� ������ ��� �����, � � ���� ���� ��� ������.
            }
        }
        else
        {
            _unspentSkillPointsText.gameObject.SetActive(false);
            foreach (TMP_Text statText in _statsList)
            {
                statText.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        _diplomacyText.text = "����������: " + _playerStats.BaseDiplomacy + 
            (_playerStats.AdditionalDiplomacy > 0 ? $" + <color=green>{_playerStats.AdditionalDiplomacy}</color>" : "" );
        _luckText.text = "�����: " + _playerStats.BaseLuck +
            (_playerStats.AdditionalLuck > 0 ? $" + <color=green>{_playerStats.AdditionalLuck}</color>" : "");
        _toughnessText.text = "���������: " + _playerStats.BaseToughness +
            (_playerStats.AdditionalToughness > 0 ? $" + <color=green>{_playerStats.AdditionalToughness}</color>" : "");
        _craftingText.text = "��������: " + _playerStats.BaseCrafting +
            (_playerStats.AdditionalCrafting > 0 ? $" + <color=green>{_playerStats.AdditionalLuck}</color>" : "");

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
