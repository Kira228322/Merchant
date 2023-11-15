using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _music;
    [SerializeField] private AudioSource _audioSource;
    private AudioClip _previousMusic = null;
    private void Start()
    {
        StartCoroutine(EndlessMusic());
    }

    private IEnumerator EndlessMusic()
    {
        AudioClip currentMusic;
        WaitForSeconds waitForSeconds;
        while (true)
        {
            currentMusic = _music[Random.Range(0, _music.Count)];
            while (currentMusic == _previousMusic)
            {
                currentMusic = _music[Random.Range(0, _music.Count)];
            }
            _audioSource.clip = currentMusic;
            _audioSource.volume = 0.1f;
            _audioSource.Play();
            _previousMusic = currentMusic;
            
            waitForSeconds = new WaitForSeconds(0.1f);
            for (int i = 0; i < 80; i++)
            {
                _audioSource.volume += 0.01f; // Громкость поднимается до 0.9
                yield return waitForSeconds;
            }
            
            waitForSeconds = new WaitForSeconds(currentMusic.length - 12);
            yield return waitForSeconds;
            waitForSeconds = new WaitForSeconds(0.1f);
            for (int i = 0; i < 40; i++)
            {
                _audioSource.volume -= 0.02f;
                yield return waitForSeconds;
            }

            waitForSeconds = new WaitForSeconds(Random.Range(30, 60));
            yield return waitForSeconds;
        }
    }
}
