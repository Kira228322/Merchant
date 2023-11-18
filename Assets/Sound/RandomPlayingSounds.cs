using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlayingSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _minDelay;
    [SerializeField] private float _maxDelay;
    [SerializeField] private List<AudioClip> _sounds;
    private float _baseVolume;
    private void Start()
    {
        _baseVolume = _audioSource.volume - 0.04f;
        StartCoroutine(PlaySound());
    }

    private IEnumerator PlaySound()
    {
        WaitForSeconds waitForSeconds;
        while (true)
        {
            waitForSeconds = new WaitForSeconds(Random.Range(_minDelay, _maxDelay));
            yield return waitForSeconds;
            _audioSource.clip = _sounds[Random.Range(0, _sounds.Count)];
            _audioSource.volume = Random.Range(_baseVolume - 0.04f, _baseVolume + 0.04f);
            _audioSource.PlayWithRandomPitch();
        }
    }
}
