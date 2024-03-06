using UnityEngine;

public static class AudioSourceExtensions
{
    public static void PlayWithRandomPitch(this AudioSource audioSource)
    {
        audioSource.pitch = Random.Range(0.91f, 1.09f);
        audioSource.Play();
    }

    public static void PlayOneShotWithRandomPitch(this AudioSource audioSource)
    {
        audioSource.pitch = Random.Range(0.91f, 1.09f);
        audioSource.PlayOneShot(audioSource.clip);
    }
}
