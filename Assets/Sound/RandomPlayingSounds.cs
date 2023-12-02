using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomPlayingSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _minDelay;
    [SerializeField] private float _maxDelay;
    [SerializeField] private List<AudioClip> _sounds;
    [SerializeField] private int _hourStart;
    [SerializeField] private int _hourEnd;
    private Coroutine _coroutine;
    
    private float _baseVolume;

    private void OnEnable()
    {
        GameTime.HourChanged += SoundStart;
    }

    private void OnDisable()
    {
        GameTime.HourChanged -= SoundStart;
    }

    private void Start()
    {
        _baseVolume = _audioSource.volume - 0.06f;
        SoundStart();
    }

    private void SoundStart()
    {
        if (_hourEnd > _hourStart)
        {
            if (GameTime.Hours >= _hourStart && GameTime.Hours <= _hourEnd)
            {
                if (_coroutine == null)
                    _coroutine = StartCoroutine(PlaySound());
                return;
            }
        }
        else if ((GameTime.Hours >= _hourStart && GameTime.Hours >= _hourEnd) || (GameTime.Hours <= _hourStart && GameTime.Hours <= _hourEnd))
        {
            if (_coroutine == null)
                _coroutine = StartCoroutine(PlaySound());
            return;
        }
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
    
    private IEnumerator PlaySound()
    {
        WaitForSeconds waitForSeconds;
        while (true)
        {
            waitForSeconds = new WaitForSeconds(Random.Range(_minDelay, _maxDelay));
            yield return waitForSeconds;
            _audioSource.clip = _sounds[Random.Range(0, _sounds.Count)];
            _audioSource.volume = Random.Range(_baseVolume, _baseVolume + 0.06f);
            _audioSource.PlayWithRandomPitch();
        }
    }
}
