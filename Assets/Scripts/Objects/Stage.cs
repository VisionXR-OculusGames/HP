using com.VisionXR.Models;
using System;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public InputDataSO inputData;
    public AudioDataSO audiodata;

    [Header(" Panel Objects")]
    public GameObject levelSpawner;
    public GameObject PauseCanvas;
    public GameObject PausePanel;

    private void OnEnable()
    {
        inputData.MenuPressedEvent += MenuPressed;
    }

    private void OnDisable()
    {
        inputData.MenuPressedEvent -= MenuPressed;
    }

    private void MenuPressed()
    {
        audiodata.PlayButtonClickSound();

        if (PauseCanvas.activeInHierarchy)
        {
            PauseCanvas.SetActive(false);
            PausePanel.SetActive(false);
            levelSpawner.SetActive(true);
        }
        else
        {
            PauseCanvas.SetActive(true);
            PausePanel.SetActive(true);
            levelSpawner.SetActive(false);
        }
    }
}
