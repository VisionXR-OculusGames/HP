using com.VisionXR.Models;
using System.Collections.Generic;
using UnityEngine;

public class HenoiLevelsPanelView: MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public AudioDataSO audioData;
    public UIDataSO uiData;
    public PlayerDataSO playerData;

    [Header(" Level Objects")]
    public List<GameObject> levelObjects;
    public List<GameObject> lockObjects;

    [Header(" Local Objects")]
    public GameObject MainCanvasObject;
    public GameObject ScoreCanvasObject;
    public GameObject HenoiScorePanel;
    public GameObject GamesPanel;

 

    public void LevelBtnClicked(int levelNo)
    {
       
            audioData.PlayButtonClickSound();

            ScoreCanvasObject.SetActive(true);
            HenoiScorePanel.SetActive(true);
            MainCanvasObject.SetActive(false);

            uiData.HenoiLevelChanged(levelNo);
        
    }


    public void BackBtnClciked()
    {
        audioData.PlayButtonClickSound();
        GamesPanel.SetActive(true);
        gameObject.SetActive(false);
    }

}
