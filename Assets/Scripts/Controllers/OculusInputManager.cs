using com.VisionXR.Models;
using UnityEngine;


public class OculusInputManager : MonoBehaviour
{

    [Header("Scriptable Objects")]
    public InputDataSO inputData;


    [Header("GameObjects")]
    public GameObject LeftPinchPoint;
    public GameObject RightPinchPoint;
    public OVRHand leftHand;
    public OVRHand rightHand;

    [Header("Local variables")]
    public float maxPinchThreshold = 0.8f;
    public float minPinchThreshold = 0.2f;

    private bool isLineStarted = false;
    private bool isTriggerDown = false;
    private bool isLeftActive = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Application.isEditor)
        {
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inputData.isInputActivated)
        {
            Process();
        }

        //if(OVRInput.GetDown(OVRInput.Button.Start))
        //{
        //    inputData.MenuPressed();
        //}
    }

    private void Process()
    {
        float rtValue = rightHand.GetFingerPinchStrength(OVRHand.HandFinger.Index);
        float ltValue = leftHand.GetFingerPinchStrength(OVRHand.HandFinger.Index);

        float tValue = 0;
       

        // Decide the active hand ONLY when pinch starts
        if (!isTriggerDown)
        {
            if (ltValue > maxPinchThreshold || rtValue > maxPinchThreshold)
            {
                isTriggerDown = true;
                isLeftActive = ltValue > rtValue;
                
            }
        }
        else
        {
            // Pinch is ongoing — use the hand that was chosen
            tValue = isLeftActive ? ltValue : rtValue;

            if (tValue < minPinchThreshold)
            {
                isTriggerDown = false;
                isLineStarted = false;

                inputData.StopPinch(
                    isLeftActive ? LeftPinchPoint.transform.position : RightPinchPoint.transform.position,
                    isLeftActive ? LeftPinchPoint.transform.rotation : RightPinchPoint.transform.rotation
                );
                return;
            }
        }

        // Pinch started
        if (isTriggerDown && !isLineStarted)
        {
            isLineStarted = true;
            inputData.StartPinch(
                isLeftActive ? LeftPinchPoint.transform.position : RightPinchPoint.transform.position,
                isLeftActive ? LeftPinchPoint.transform.rotation : RightPinchPoint.transform.rotation
            );

  
        }
        // Pinch continuing
        else if (isTriggerDown && isLineStarted)
        {
            inputData.ContinuePinch(
                isLeftActive ? LeftPinchPoint.transform.position : RightPinchPoint.transform.position,
                isLeftActive ? LeftPinchPoint.transform.rotation : RightPinchPoint.transform.rotation
            );
        }

    }
}


