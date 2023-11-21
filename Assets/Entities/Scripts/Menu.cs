using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private Slider _soundslider;
    [SerializeField] private Slider _Musicslider;
    [SerializeField] private Slider _playerPanelslider;
    [SerializeField] private AudioMixerGroup _sound;
    [SerializeField] private AudioMixerGroup _music;
    [SerializeField] private AudioMixerGroup _questSound;

    [SerializeField] private RectTransform _leftButton;
    [SerializeField] private RectTransform _rightButton;
    private Image _leftButtonImage;
    private Image _rightButtonImage;

    [SerializeField] private Animator _animator;
    private float _PlayerMovePanelsTimer;
    private Coroutine FadeIn;
    private Coroutine FadeOut;

    [SerializeField] private AudioMixerSnapshot Normal;
    [SerializeField] private AudioMixerSnapshot InMenu;
    private void Awake()
    {
        _leftButtonImage = _leftButton.gameObject.GetComponent<Image>();
        _rightButtonImage = _rightButton.gameObject.GetComponent<Image>();
        
        // TODO задать всем 3 слайдерам сохранненое значение
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
        float value = math.lerp(-80, 0, (float)Math.Pow(_soundslider.value, 0.33f));
        _sound.audioMixer.SetFloat("SoundsVolume", value);
    }

    public void OnMusicValueChange()
    {
        float value = math.lerp(-80, 0, (float)Math.Pow(_Musicslider.value, 0.33f));
        _music.audioMixer.SetFloat("MusicVolume", value);
        _questSound.audioMixer.SetFloat("QuestVolume", value);
    }

    public void OnPlayerMovePanelValueChange()
    {
        float value = math.lerp(0, 490, _playerPanelslider.value);
        _leftButton.sizeDelta = new Vector2(value, _leftButton.sizeDelta.y);
        _rightButton.sizeDelta = new Vector2(value, _rightButton.sizeDelta.y);
        
        _PlayerMovePanelsTimer = 2.2f;
        
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
        while (_PlayerMovePanelsTimer >= 0)
        {
            _PlayerMovePanelsTimer -= 0.05f;
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
}
