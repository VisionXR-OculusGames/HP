using com.VisionXR.Models;
using UnityEngine;

public class GamesPanelView : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public AudioDataSO audioData;
    public UIDataSO uiData;

    [Header(" Panel Objects")]
   
    public GameObject matchStickLevelspanel;
    public GameObject brainVitaLevelspanel;
    public GameObject henoiLevelspanel;
    public GameObject slideTheBlockLevelsPanel;
    public GameObject tangramLevelsPanel;

    public void MatchStickGameClicked()
    {
        audioData.PlayButtonClickSound();
        ResetPanels();
        uiData.SetGame(Games.MatchStick);
        matchStickLevelspanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void BraiVitaClicked()
    {
        audioData.PlayButtonClickSound();
        ResetPanels();
        uiData.SetGame(Games.BrainVita);
        brainVitaLevelspanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void TowersOfHenoiClicked()
    {
        audioData.PlayButtonClickSound();
        ResetPanels();
        uiData.SetGame(Games.TowersOfHenoi);
        henoiLevelspanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SlideTheBlockClicked()
    {
        audioData.PlayButtonClickSound();
        ResetPanels();
        uiData.SetGame(Games.SlideTheBlock);
        slideTheBlockLevelsPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void TangramClicked()
    {
        audioData.PlayButtonClickSound();
        ResetPanels();
        uiData.SetGame(Games.Tangram);
        tangramLevelsPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    private void ResetPanels()
    {
        matchStickLevelspanel.SetActive(false);
        brainVitaLevelspanel.SetActive(false);
        henoiLevelspanel.SetActive(false);
        slideTheBlockLevelsPanel.SetActive(false);
    }
}
