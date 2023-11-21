using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Menu : MonoBehaviour, ISaveable<MenuSaveData>
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _playerPanelSlider;
    [SerializeField] private AudioMixerGroup _sound;
    [SerializeField] private AudioMixerGroup _music;
    [SerializeField] private AudioMixerGroup _questSound;

    [SerializeField] private RectTransform _leftButton;
    [SerializeField] private RectTransform _rightButton;
    private Image _leftButtonImage;
    private Image _rightButtonImage;

    [SerializeField] private Animator _animator;
    private float _playerMovePanelsTimer;
    private Coroutine FadeIn;
    private Coroutine FadeOut;

    [SerializeField] private AudioMixerSnapshot Normal;
    [SerializeField] private AudioMixerSnapshot InMenu;
    private void Awake()
    {
        _leftButtonImage = _leftButton.gameObject.GetComponent<Image>();
        _rightButtonImage = _rightButton.gameObject.GetComponent<Image>();
    }

    public void OnMenuButtonClick()
    {
        GameTime.SetTimeScale(0);
        InMenu.TransitionTo(0.9f);
        _menuPanel.SetActive(true);
        _animator.SetTrigger("FadeIn");
    }

    public void OnExitMenuButtonClick()
    {
        GameTime.SetTimeScale(1);
        Normal.TransitionTo(0.9f);
        StopAllCoroutines();
        if (_leftButtonImage.color.a != 0)
            StartCoroutine(FadeOutPlayerMovePanels());
        _animator.SetTrigger("FadeOut");
    }

    public void Disable()
    {
        _menuPanel.SetActive(false);
    }

    
    public void OnSoundValueChange()
    {
        float value = math.lerp(-80, 0, _soundSlider.value);
        _sound.audioMixer.SetFloat("SoundsVolume", value);
    }

    public void OnMusicValueChange()
    {
        float value = math.lerp(-80, 0, _musicSlider.value);
        _music.audioMixer.SetFloat("MusicVolume", value);
        _questSound.audioMixer.SetFloat("QuestVolume", value);
    }

    public void OnPlayerMovePanelValueChange()
    {
        float value = math.lerp(0, 490, _playerPanelSlider.value);
        _leftButton.sizeDelta = new Vector2(value, _leftButton.sizeDelta.y);
        _rightButton.sizeDelta = new Vector2(value, _rightButton.sizeDelta.y);
        
        _playerMovePanelsTimer = 2.2f;
        
        if (FadeIn == null)
        {
            if (FadeOut != null)
            {
                StopCoroutine(FadeOut);
                FadeOut = null;
            }
            FadeIn = StartCoroutine(FadeInPlayerMovePanels());
            StartCoroutine(Timer());
        }
    }

    private IEnumerator Timer()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.05f);
        while (_playerMovePanelsTimer >= 0)
        {
            _playerMovePanelsTimer -= 0.05f;
            yield return waitForSeconds;
        }

        FadeOut = StartCoroutine(FadeOutPlayerMovePanels());
    }

    private IEnumerator FadeInPlayerMovePanels()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);

        Color color = _leftButtonImage.color;
        color.a = 0.05f;
        for (int i = 0; i < 30; i++)
        {
            color.a += 0.02f;
            _leftButtonImage.color = color;
            _rightButtonImage.color = color;
            yield return waitForSeconds;
        }
        color.a = 0.65f;
        _leftButtonImage.color = color;
        _rightButtonImage.color = color;
    }

    private IEnumerator FadeOutPlayerMovePanels()
    {
        FadeIn = null;
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);
        
        Color color = _leftButtonImage.color;
        color.a = 0.65f;
        for (int i = 0; i < 20; i++)
        {
            color.a -= 0.03f;
            _leftButtonImage.color = color;
            _rightButtonImage.color = color;
            yield return waitForSeconds;
        }

        color.a = 0;
        _leftButtonImage.color = color;
        _rightButtonImage.color = color;
    }

    public MenuSaveData SaveData()
    {
        MenuSaveData saveData = new(_soundSlider.value, _musicSlider.value, _playerPanelSlider.value);
        return saveData;
    }

    public void LoadData(MenuSaveData data)
    {
        _soundSlider.value = data.SoundSliderValue;
        _musicSlider.value = data.MusicSliderValue;
        _playerPanelSlider.SetValueWithoutNotify(data.PlayerPanelsValue); //удивительная функция не триггерит ивенты слайдера, не вызывая OnPlayerMovePanelValueChange()

     
        //TODO информация для размышления:
        //Настройки не будут загружены, пока игрок не нажмёт продолжить игру, из-за принципов на которых у нас строится сохранение.
        //То есть потенциально, игрок может оглушаться каждый раз при запуске игры.
        //Это нехорошо, и есть вариант либо сделать дефолтные значения тихими,
        //либо исследовать другие методы сохранения только для настроек
    }
}
