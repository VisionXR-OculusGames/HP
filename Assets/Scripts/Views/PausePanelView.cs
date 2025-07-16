using com.VisionXR.Models;
using UnityEngine;

public class PausePanelView : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public UIDataSO uiData;
    public AudioDataSO audiodata;


    [Header(" Game Objects")]
    public GameObject levelSpawner;
    public GameObject pausePanel;
    public GameObject successCanvas;

    public void QuitBtnClicked()
    {
        audiodata.PlayButtonClickSound();
        uiData.HomeClicked();
    }

    public void PauseBtnClicked()
    {
        audiodata.PlayButtonClickSound();
        pausePanel.SetActive(true);
        levelSpawner.SetActive(false);
    }
    public void ResumeBtnClicked()
    {
        audiodata.PlayButtonClickSound();
        pausePanel.SetActive(false);
        levelSpawner.SetActive(true);
    }

   
}
