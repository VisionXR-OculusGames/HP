using com.VisionXR.Models;
using UnityEngine;


public class KeyBoardInputManager : MonoBehaviour
{

    [Header("Scriptable Objects")]
    public InputDataSO inputSO;
    public GameObject RightPoint;
  

    [Header("Key Codes")]
    public KeyCode triggerDownCode = KeyCode.Space;
    public KeyCode triggerUpCode = KeyCode.Return;
    public KeyCode gripDownCode = KeyCode.DownArrow;
    public KeyCode gripUpCode = KeyCode.UpArrow;
    public KeyCode pauseCode = KeyCode.P;


    [Header("Variables")]
    private bool isLineStarted = false;
    private bool isTriggerDown = false;
   
    private void Start()
    {
        if (!Application.isEditor)
        {
            this.enabled = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        ProcessKeyboardData();
    }

    private void ProcessKeyboardData()
    {
        if (inputSO.isInputActivated)
        {

            if (Input.GetKeyDown(triggerDownCode))
            {
                isTriggerDown = true;
            }

            else if (Input.GetKeyDown(triggerUpCode))
            {
                isTriggerDown = false;
            }


            if (isTriggerDown && !isLineStarted)
            {
                isLineStarted = true;
                inputSO.StartPinch(RightPoint.transform.position, RightPoint.transform.rotation);
                Debug.Log(" pinch start ");
            }

            else if (isTriggerDown && isLineStarted)
            {
                
                inputSO.ContinuePinch(RightPoint.transform.position, RightPoint.transform.rotation);

            }

            else if (!isTriggerDown && isLineStarted)
            {
                isLineStarted = false;
                inputSO.StopPinch(RightPoint.transform.position, RightPoint.transform.rotation);

            }        

        }


        if (Input.GetKeyDown(pauseCode))
        {
            inputSO.MenuPressed();
            
        }

    }
}
