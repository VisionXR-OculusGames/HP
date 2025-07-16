using com.VisionXR.Models;
using System.Collections;
using UnityEngine;

public class NavigationPanel : MonoBehaviour
{
    [Header(" Scriptable Objects ")]
    public AudioDataSO audiodata;
    public string tutorialKey = "HPKey";

    [Header(" Panels ")]
    public GameObject GamesPanel;
    public GameObject LeaderBoardPanel;
    public GameObject SettingsPanel;
    public GameObject TutorialPanel;
    public GameObject ReviewPanel;

    [Header(" Images ")]
    public GameObject GamesImage;
    public GameObject LeaderBoardImage;
    public GameObject SettingsImage;
    public GameObject TutorialImage;
    public GameObject ReviewImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public IEnumerator Start()
    {
        yield return new WaitForSeconds(2);

        if(PlayerPrefs.HasKey(tutorialKey))
        {
            ResetImages();
            ResetPanels();
            GamesPanel.SetActive(true);
            GamesImage.SetActive(true);
        }
        else
        {
            ResetImages();
            ResetPanels();
            TutorialImage.SetActive(true);
            TutorialPanel.SetActive(true);
            PlayerPrefs.SetString(tutorialKey, "true");
        }
    }


    public void GamesBtnClicked()
    {
        audiodata.PlayButtonClickSound();
        ResetPanels();
        ResetImages();
        GamesPanel.SetActive(true);
        GamesImage.SetActive(true);
    }

    public void LeaderBoardBtnClicked()
    {
        audiodata.PlayButtonClickSound();
        ResetPanels();
        ResetImages();
        LeaderBoardPanel.SetActive(true );
        LeaderBoardImage.SetActive(true );
    }

    public void SettingsBtnClicked()
    {
        audiodata.PlayButtonClickSound();
        ResetPanels();
        ResetImages();
        SettingsPanel.SetActive(true );
        SettingsImage.SetActive(true );
    }

    public void TutorialBtnClicked()
    {
        audiodata.PlayButtonClickSound();
        ResetPanels();
        ResetImages();
        TutorialPanel.SetActive(true );
        TutorialImage.SetActive(true );
    }

    public void ReviewBtnClicked()
    {
        audiodata.PlayButtonClickSound();
        ResetPanels();
        ResetImages();
        ReviewPanel.SetActive(true);
        ReviewImage.SetActive(true);
    }

    void ResetPanels()
    {
        GamesPanel.SetActive(false);
        LeaderBoardPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        TutorialPanel.SetActive(false);
        ReviewPanel.SetActive(false);
    }

    void ResetImages()
    {
        GamesImage.SetActive(false);
        LeaderBoardImage.SetActive(false);
        SettingsImage.SetActive(false);
        TutorialImage.SetActive(false);
        ReviewImage.SetActive(false);
    }
}
