using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickNoise;
    public void ClickSound()
    {
        audioSource.Stop();
        audioSource.clip = clickNoise;
        audioSource.Play();
    }
}
