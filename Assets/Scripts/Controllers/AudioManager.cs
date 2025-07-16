using com.VisionXR.Models;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public AudioDataSO audioData;

    [Header(" Audio Objects")]
    public List<AudioSource> audioSources;
    


    private void OnEnable()
    {
        audioData.ButtonClickSoundEvent += PlayButtonClickSound;
        audioData.LevelCompletedSoundEvent += PlayWinningSound;
        audioData.GrabSoundEvent += PlayGrabSound;
        audioData.UnGrabSoundEvent += PlayUnGrabSound;
   
    }
    private void OnDisable()
    {
        audioData.ButtonClickSoundEvent -= PlayButtonClickSound;
        audioData.LevelCompletedSoundEvent -= PlayWinningSound;
        audioData.GrabSoundEvent -= PlayGrabSound;
        audioData.UnGrabSoundEvent -= PlayUnGrabSound;
    }

    public void PlayButtonClickSound()
    {
        audioSources[0].Play();
    }

    public void PlayWinningSound()
    {
        audioSources[1].Play();
    }

    public void PlayGrabSound()
    {
        audioSources[2].Play();
    }

    public void PlayUnGrabSound()
    {
        audioSources[3].Play();
    }

}
