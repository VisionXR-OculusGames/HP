using com.VisionXR.Models;
using System;
using TMPro;
using UnityEngine;

public class HenoiScorePanelView : MonoBehaviour
{
    [Header(" Scriptable Object")]
    public UIDataSO uiData;
    public AudioDataSO audioData;
    public LevelDataSO levelData;

    [Header(" Canvas Object")]    
    public GameObject ScoreCanvas;
    public GameObject MainCanvas;
    public GameObject HenoiScorePanel;

    [Header(" Text Object")]
    public TMP_Text levelText;
    public TMP_Text movesText;

    private void OnEnable()
    {
       
        levelData.SetLevelEvent += ShowLevel;
        levelData.SetMovesEvent += ShowMoves;
    }

    private void OnDisable()
    {
        levelData.SetLevelEvent -= ShowLevel;
        levelData.SetMovesEvent -= ShowMoves;
    }

    private void ShowMoves(int moves)
    {
        movesText.text = " Moves : " + moves;
    }

    private void ShowLevel(int level)
    {
        levelText.text = " Level : " + level; 
    }
    public void CloseBtnClicked()
    {
        audioData.PlayButtonClickSound();
        uiData.ExitGame();
        MainCanvas.SetActive(true);
        ScoreCanvas.SetActive(false);
        HenoiScorePanel.SetActive(false);
    }

    public void NextBtnClicked()
    {
        audioData.PlayButtonClickSound();
        levelData.LoadNextLevel(levelData.HenoiLevelNo + 1);
    }

    public void PreviousBtnClicked()
    {
        audioData.PlayButtonClickSound();
        levelData.LoadNextLevel(levelData.HenoiLevelNo - 1);
    }

    public void ReplayBtnClicked()
    {
        audioData.PlayButtonClickSound();
        movesText.text = " Moves : " + 0;
    }
}
