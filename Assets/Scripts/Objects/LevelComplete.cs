using com.VisionXR.Models;
using System;
using System.Collections;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public LevelDataSO leveldata;
    public UIDataSO uidata;
    public AudioDataSO audiopData;

    [Header(" Level Objects")]
    public GameObject levelSpawner;


    [Header(" Canvas Objects ")]
    public GameObject Scorecanvas;
    public GameObject Successcanvas;
    public GameObject Maincanvas;

    [Header("score  panel Objects ")]
    public GameObject matchStickScorepanel;
    public GameObject brainVitaScorepanel;
    public GameObject henoiScorepanel;
    public GameObject slideTheBlockScorepanel;

    [Header("success  panel Objects ")]
    public GameObject successpanel;
    public GameObject pausepanel;
    public GameObject allLevelsCompletePanel;

    [Header("main  panel Objects ")]
    public GameObject gamesPanel;
    public GameObject mainMatchStickPanel;
    public GameObject mainBlockPanel;
    public GameObject mainBrainvitaPanel;
    public GameObject matchStickLevelsPanel;
    public GameObject blockLevelsPanel;
    public GameObject henoiLevelsPanel;
    public GameObject brainvitaLevelsPanel;


    private void OnEnable()
    {
        leveldata.MatchStickLevelSuccesEvent += MatchStickSuccess;
        leveldata.BlockLevelSuccesEvent += BlockLevelSuccess;
        leveldata.HenoiLevelSuccesEvent += HenoiLevelSuccess;
        leveldata.BraivitaLevelSuccesEvent += BrainvitaSuccess;

        uidata.AllLevelsCompleteEvent += ShowAllLevelsCompletePanel;
        uidata.HomeButtonClickedEvent += HomeBtnClicked;


    }

    private void OnDisable()
    {
        leveldata.MatchStickLevelSuccesEvent -= MatchStickSuccess;
        leveldata.BlockLevelSuccesEvent -= BlockLevelSuccess;
        leveldata.HenoiLevelSuccesEvent -= HenoiLevelSuccess;
        leveldata.BraivitaLevelSuccesEvent -= BrainvitaSuccess;

        uidata.AllLevelsCompleteEvent -= ShowAllLevelsCompletePanel;
        uidata.HomeButtonClickedEvent -= HomeBtnClicked;

    }



    private void ShowAllLevelsCompletePanel()
    {
        Resetpanels();
        Successcanvas.SetActive(true);
        allLevelsCompletePanel.SetActive(true);
    }

    public void HomeBtnClicked()
    {
        audiopData.PlayButtonClickSound();
        Resetpanels();
        ResetMainPanels();
        Maincanvas.SetActive(true);
        gamesPanel.SetActive(true);
        levelSpawner.SetActive(true);
        if(levelSpawner.transform.childCount > 0)
        {
            Destroy(levelSpawner.transform.GetChild(0).gameObject);
        }

    }

    private void BrainvitaSuccess(int levelNo)
    {
        StartCoroutine(WaitAndShow());
    }
    private void HenoiLevelSuccess(int levelNo)
    {
        StartCoroutine(WaitAndShow());
    }

    private void BlockLevelSuccess(int levelNo)
    {
        StartCoroutine(WaitAndShow());
    }

    private void MatchStickSuccess(int levelNo)
    {
        StartCoroutine(WaitAndShow());
    }

    private IEnumerator WaitAndShow()
    {
        yield return new WaitForSeconds(2);
        Resetpanels();
        Successcanvas.SetActive(true);
        successpanel.SetActive(true);
    }

    private void Resetpanels()
    {

        matchStickScorepanel.SetActive(false);     
        brainVitaScorepanel.SetActive(false);
        henoiScorepanel.SetActive(false);
        slideTheBlockScorepanel.SetActive(false);

        successpanel.SetActive(false);
        pausepanel.SetActive(false);
        allLevelsCompletePanel.SetActive(false);

        Scorecanvas.SetActive(false);
        Successcanvas.SetActive(false);
    }

    private void ResetMainPanels()
    {
        gamesPanel.SetActive(false);
        mainMatchStickPanel.SetActive(false);
        mainBrainvitaPanel.SetActive(false);
        mainBlockPanel.SetActive(false);
        matchStickLevelsPanel.SetActive(false);
        blockLevelsPanel.SetActive(false);
        henoiLevelsPanel.SetActive(false);
        brainvitaLevelsPanel.SetActive(false);
    }
}
