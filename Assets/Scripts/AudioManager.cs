using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class AudioManager : MonoBehaviour
{
    public float mainVolume, sfxVolume, musicVolume;
    public AudioMixer audioMixer;
    public Slider main, sfx, music;

    public AudioSource playing;
    public AudioClip track0;
    public AudioClip track1;
    public AudioClip track2;
    public int currentMusicTrack = 0;

    private void Start()
    {
        AudioVariables();

        currentMusicTrack = SaveSystem.LoadData("currentTrack");

        string sceneName = SceneManager.GetActiveScene().name;

        //Checks if in game scene to change music to user defined option
        if (sceneName == "Game")
        {
            if (currentMusicTrack == 0)
            {
                playing.clip = track0;
                playing.Play();
            }
            else if (currentMusicTrack == 1)
            {
                playing.clip = track1;
                playing.Play();
            }
            else if (currentMusicTrack == 2)
            {
                playing.clip = track2;
                playing.Play();
            }
        }
    }
    public void AudioVariables()
    {
        SaveSystem.LoadAudio(this);
        audioMixer.SetFloat("Volume", mainVolume); ;
        audioMixer.SetFloat("MusicVolume", musicVolume); ;
        audioMixer.SetFloat("SFXVolume", sfxVolume); ;
        main.value = mainVolume;
        sfx.value = sfxVolume;
        music.value = musicVolume;
    }
}
