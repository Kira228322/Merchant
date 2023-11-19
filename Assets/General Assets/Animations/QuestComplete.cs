using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class QuestComplete : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private TMP_Text _tmpText;
    [SerializeField] private AudioMixerGroup _audioMusic;
    [SerializeField] private AudioMixerGroup _audioSound;
    private void OnEnable()
    {
        _audioMusic.audioMixer.GetFloat("MusicParentVolume", out var MusicParentVolume);
        _audioMusic.audioMixer.SetFloat("MusicParentVolume", MusicParentVolume - 3f);
        _audioSound.audioMixer.GetFloat("SoundParentVolume", out var SoundParentVolume);
        _audioSound.audioMixer.SetFloat("SoundParentVolume", SoundParentVolume - 3f);
        _audioSource.Play();
    }

    private void OnDisable()
    {
        _audioMusic.audioMixer.GetFloat("MusicParentVolume", out var MusicParentVolume);
        _audioMusic.audioMixer.SetFloat("MusicParentVolume", MusicParentVolume + 3f);
        _audioSound.audioMixer.GetFloat("SoundParentVolume", out var SoundParentVolume);
        _audioSound.audioMixer.SetFloat("SoundParentVolume", SoundParentVolume + 3f);
    }

    public void ChangeText(string textName)
    {
        _tmpText.text = $"{textName}\nВыполнено";
        enabled = true;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
