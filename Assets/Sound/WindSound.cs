using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class WindSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [HideInInspector] public float Volume; 
    private void Start()
    {
        Volume = 0.7f;
        StartCoroutine(PlayWindSound());
    }

    private IEnumerator PlayWindSound()
    {
        WaitForSeconds waitForSeconds;
        while (true)
        {
            waitForSeconds = new WaitForSeconds(Random.Range(25,56));
            yield return waitForSeconds;
            _audioSource.volume = Volume;
            _audioSource.PlayWithRandomPitch();
        }
    }
}
