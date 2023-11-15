using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioSourceExtensions 
{
    public static void PlayWithRandomPitch(this AudioSource audioSource)
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();
    }
}
