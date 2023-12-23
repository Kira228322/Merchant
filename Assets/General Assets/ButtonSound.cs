using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void PlayButtonSound()
    {
        GameManager.Instance._ButtonAudioSource.PlayOneShot(GameManager.Instance._ButtonAudioSource.clip);
    }
    
}
