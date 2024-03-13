using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _playerPanelSlider;
    [SerializeField] private AudioMixerGroup _sound;
    [SerializeField] private AudioMixerGroup _music;
    [SerializeField] private AudioMixerGroup _questSound;
    [SerializeField] private AudioMixerGroup _UI;

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

    [Header("Default values")]
    [SerializeField][Range(0, 1)] private float _defaultSoundMultiplier;
    [SerializeField][Range(0, 1)] private float _defaultMusicMultiplier;
    [SerializeField][Range(0, 1)] private float _defaultPlayerMoveButtonSizeMultiplier;

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
        if (MapManager.IsActiveSceneTravel)
        {
            if (!MapManager.EventInTravelIsActive)
                GameTime.SetTimeScale(GameTime.TimeScaleInTravel);
        }
        else
            GameTime.SetTimeScale(1);
        Normal.TransitionTo(0.9f);
        StopAllCoroutines();
        if (_leftButtonImage.color.a != 0)
            StartCoroutine(FadeOutPlayerMovePanels());
        _animator.SetTrigger("FadeOut");

        SaveData();
    }

    public void Disable()
    {
        _menuPanel.SetActive(false);
    }


    public void OnSoundValueChange()
    {
        float value = math.lerp(-80, 0, (float)Math.Pow(_soundSlider.value, 0.33f));
        _sound.audioMixer.SetFloat("SoundsVolume", value);
        _UI.audioMixer.SetFloat("UIVolume", value);
    }

    public void OnMusicValueChange()
    {
        float value = math.lerp(-80, 0, (float)Math.Pow(_musicSlider.value, 0.33f));
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

        SaveData();
    }

    private IEnumerator Timer()
    {
        WaitForSeconds waitForSeconds = new(0.05f);
        while (_playerMovePanelsTimer >= 0)
        {
            _playerMovePanelsTimer -= 0.05f;
            yield return waitForSeconds;
        }

        FadeOut = StartCoroutine(FadeOutPlayerMovePanels());
    }

    private IEnumerator FadeInPlayerMovePanels()
    {
        WaitForSeconds waitForSeconds = new(0.02f);

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
        WaitForSeconds waitForSeconds = new(0.02f);

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

    public void SaveData() //Не путать с ISaveable.SaveData - здесь только PlayerPrefs
    {
        PlayerPrefs.SetFloat("SavedSoundVolume", _soundSlider.value);
        PlayerPrefs.SetFloat("SavedMusicVolume", _musicSlider.value);
        PlayerPrefs.SetFloat("SavedMovePanelSize", _playerPanelSlider.value);
    }

    public void LoadData()
    {
        if (!PlayerPrefs.HasKey("SavedSoundVolume") ||
            !PlayerPrefs.HasKey("SavedMusicVolume") ||
            !PlayerPrefs.HasKey("SavedMovePanelSize"))
        {
            _soundSlider.SetValueWithoutNotify(_defaultSoundMultiplier);
            _musicSlider.SetValueWithoutNotify(_defaultMusicMultiplier);
            _playerPanelSlider.SetValueWithoutNotify(_defaultPlayerMoveButtonSizeMultiplier);
        }
        else
        {
            _soundSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SavedSoundVolume"));
            _musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SavedMusicVolume"));
            _playerPanelSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SavedMovePanelSize")); //удивительная функция не триггерит ивенты слайдера, не вызывая OnPlayerMovePanelValueChange()
        }

        OnMusicValueChange();
        OnSoundValueChange();
    }
}
