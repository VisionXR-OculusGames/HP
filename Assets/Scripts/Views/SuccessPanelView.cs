using com.VisionXR.Models;
using UnityEngine;

public class SuccessPanelView : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public UIDataSO uiData;
    public LevelDataSO levelData;
    public AudioDataSO audiodata;

    [Header(" Game Objects")]
    public GameObject levelSpawner;

    [Header(" Canvas Objects")]

    public GameObject successCanvas;
    public GameObject scoreCanvas;
    public GameObject mainCanvas;

    [Header(" Panel Objects")]
    public GameObject gamesPanel;
    public GameObject successPanel;
    public GameObject matchStickPanel;
    public GameObject brainVitaPanel;
    public GameObject henoiPanel;
    public GameObject slideTheBlockPanel;
    public GameObject tangramSuccessPanel;



    public void NextBtnClicked()
    {
        audiodata.PlayButtonClickSound();
        ResetObjects();

        scoreCanvas.SetActive(true);

        if(uiData.currentGame == Games.MatchStick)
        {
            matchStickPanel.SetActive(true);
            levelData.LoadNextLevel(levelData.MatchStickLevelNo + 1);
           
        }
        else if(uiData.currentGame == Games.SlideTheBlock)
        {
            slideTheBlockPanel.SetActive(true);
            levelData.LoadNextLevel(levelData.BlockLevelNo + 1);
           
        }
        else if (uiData.currentGame == Games.TowersOfHenoi)
        {
            henoiPanel.SetActive(true);
            levelData.LoadNextLevel(levelData.HenoiLevelNo + 1);
           
        }
        else if (uiData.currentGame == Games.BrainVita)
        {
            brainVitaPanel.SetActive(true);
            levelData.LoadNextLevel(levelData.BrainVitaLevelNo + 1);         
        }
        else if (uiData.currentGame == Games.Tangram)
        {
            tangramSuccessPanel.SetActive(true);
            levelData.LoadNextLevel(levelData.TangramLevelNo + 1);
        }

    }

    public void PreviosBtnBtnClicked()
    {
        audiodata.PlayButtonClickSound();

        ResetObjects();
        scoreCanvas.SetActive(true);

        if (uiData.currentGame == Games.MatchStick)
        {
            matchStickPanel.SetActive(true);
            levelData.LoadNextLevel(levelData.MatchStickLevelNo - 1);         
        }
        else if (uiData.currentGame == Games.SlideTheBlock)
        {
            slideTheBlockPanel.SetActive(true);
            levelData.LoadNextLevel(levelData.BlockLevelNo - 1);          
        }
        else if (uiData.currentGame == Games.TowersOfHenoi)
        {
            henoiPanel.SetActive(true);
            levelData.LoadNextLevel(levelData.HenoiLevelNo - 1);           
        }
        else if (uiData.currentGame == Games.BrainVita)
        {
            brainVitaPanel.SetActive(true);
            levelData.LoadNextLevel(levelData.BrainVitaLevelNo - 1);          
        }
        else if (uiData.currentGame == Games.Tangram)
        {
            tangramSuccessPanel.SetActive(true);
            levelData.LoadNextLevel(levelData.TangramLevelNo - 1);
        }


    }

    public void QuitBtnBtnClicked()
    {
        audiodata.PlayButtonClickSound();

        ResetObjects();

        mainCanvas.SetActive(true);
        gamesPanel.SetActive(true);

        uiData.ExitGame();
    }

    private void ResetObjects()
    {
        mainCanvas.SetActive(false);
        scoreCanvas.SetActive(false);
        successCanvas.SetActive(false);

        gamesPanel.SetActive(false);

        successPanel.SetActive(false);

        matchStickPanel.SetActive(false);
        brainVitaPanel.SetActive(false);
        henoiPanel.SetActive(false);
        slideTheBlockPanel.SetActive(false);

        tangramSuccessPanel.SetActive(false);
    }
}
