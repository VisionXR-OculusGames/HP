using System;
using UnityEngine;

namespace com.VisionXR.Models
{
    [CreateAssetMenu(fileName = "AudioDataSO", menuName = "ScriptableObjects/AudioDataSO", order = 1)]
    public class AudioDataSO : ScriptableObject
    {
        

        // Actions
        public Action ButtonClickSoundEvent;
        public Action LevelCompletedSoundEvent;
        public Action GrabSoundEvent;
        public Action UnGrabSoundEvent;

        // Methods

        public void PlayButtonClickSound()
        {
            ButtonClickSoundEvent?.Invoke();
        }

        public void PlayLevelCompletedSound()
        {
            LevelCompletedSoundEvent?.Invoke();
        }

        public void PlayGrabSound()
        {
            GrabSoundEvent?.Invoke();
        }

        public void PlayUnGrabSound()
        {
            UnGrabSoundEvent?.Invoke();
        }
     
    }
}
