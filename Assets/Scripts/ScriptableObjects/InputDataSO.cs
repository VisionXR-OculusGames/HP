using System;
using UnityEngine;

namespace com.VisionXR.Models
{
    [CreateAssetMenu(fileName = "InputSO", menuName = "ScriptableObjects/InputSO", order = 1)]
    public class InputDataSO : ScriptableObject
    {

        // variables
        public bool isInputActivated = true;

        // Actions
        public Action<Vector3,Quaternion> PinchStartEvent;
        public Action<Vector3,Quaternion> PinchContinueEvent;
        public Action<Vector3,Quaternion> PinchEndEvent;
        public Action MenuPressedEvent;
       

        // Methods

        private void OnEnable()
        {
            isInputActivated = true;
        }

        public void ActivateInput()
        {
            isInputActivated = true;
        }

        public void DeactivateInput()
        {
            isInputActivated = false;
        }

        public void StartPinch(Vector3 pos,Quaternion rot)
        {
            PinchStartEvent?.Invoke(pos, rot);
           
        }

        public void ContinuePinch(Vector3 pos,Quaternion rot)
        {
            PinchContinueEvent?.Invoke(pos,rot);

        }
        public void StopPinch(Vector3 pos,Quaternion rot)
        {
           PinchEndEvent?.Invoke(pos, rot);
        
        }    

        public void MenuPressed()
        {
            MenuPressedEvent?.Invoke();
        }

    }
}
