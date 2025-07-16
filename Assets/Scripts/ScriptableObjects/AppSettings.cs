using System;
using UnityEngine;

namespace com.VisionXR.Models
{

    [CreateAssetMenu(fileName = "AppSettings", menuName = "ScriptableObjects/AppSettings", order = 1)]
    public class AppSettings : ScriptableObject
    {

        [Header(" Button Colors ")]
        public Color SelectedColor;
        public Color HoverColor;
        public Color HoverIdle;
        public Color IdleColor;
        public Color OtherHoverColor;
        public Color OtherIdleColor;


        [Header(" Haptic Variables ")]
        public float vibrationDuration = 0.1f;
        public float vibrationAmplitude = 0.1f;

        
        // Actions
        public Action SettingsChangedEvent;
        public Action<bool> ShowRoomPropertiesEvent;
        public Action LineColorChangedEvent;
        public Action<float> LineWidthChangedEvent;


        public void SetData()
        {
           

        }
    

        public void GetAppData()
        {
          
        }


        public void SaveAppData()
        {
            
        }
        public void LoadAppData()
        {
       
        }

 

    }
}

