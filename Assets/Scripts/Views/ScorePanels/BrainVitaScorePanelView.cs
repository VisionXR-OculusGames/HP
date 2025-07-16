using com.VisionXR.Models;
using System;
using TMPro;
using UnityEngine;

public class BrainVitaScorePanelView : MonoBehaviour
{
    [Header(" Scriptable Object")]
    public UIDataSO uiData;
    public AudioDataSO audioData;
    public LevelDataSO levelData;

    [Header(" Canvas Object")]
    public GameObject ScoreCanvas;
    public GameObject MainCanvas;
    public GameObject BrainVitaScorePanel;

    [Header(" Text Objects")]
    public TMP_Text MovesText;
    public TMP_Text MinMarblesText;
    public TMP_Text ScoreText;


    private void OnEnable()
    {
        levelData.SetMovesEvent += SetMoves;
        levelData.SetMinMarblesEvent += SetMinimumMarbles;
    }

    private void OnDisable()
    {
        levelData.SetMovesEvent -= SetMoves;
        levelData.SetMinMarblesEvent -= SetMinimumMarbles;
    }

    private void SetMinimumMarbles(int marbles)
    {
        MinMarblesText.text = " Min Marbles : " + marbles;
    }

    private void SetMoves(int moves)
    {
        MovesText.text = " Moves : " + moves;
    }

    public void CloseBtnClicked()
    {
        audioData.PlayButtonClickSound();
        uiData.ExitGame();
        MainCanvas.SetActive(true);
        ScoreCanvas.SetActive(false);
        BrainVitaScorePanel.SetActive(false);
    }

    public void ReplayBtnClicked()
    {
        audioData.PlayButtonClickSound();
        MovesText.text = " Moves : " + 0;
    }
}
