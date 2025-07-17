using com.VisionXR.Models;
using TMPro;
using UnityEngine;

public class TangramScorePanelView : MonoBehaviour
{
    [Header("Scriptable Object")]
    public UIDataSO uiData;
    public AudioDataSO audioData;
    public LevelDataSO levelData;

    [Header("Canvas Object")]
    public GameObject ScoreCanvas;
    public GameObject MainCanvas;
    public GameObject TangramLevel;

    [Header("Text Object")]
    public TMP_Text levelText;
    public TMP_Text movesText;

    private void OnEnable()
    {
        levelData.SetTangramMovesEvent += ShowMoves;
        levelData.SetLevelEvent += ShowLevel;
    }

    private void OnDisable()
    {
        levelData.SetTangramMovesEvent -= ShowMoves;
        levelData.SetLevelEvent -= ShowLevel;
    }

    private void ShowMoves(int moves, int minMoves)
    {
        movesText.text = "Moves : " + moves + " / " + minMoves;
    }

    private void ShowLevel(int level)
    {
        levelText.text = "Level : " + level;
    }

    public void CloseBtnClicked()
    {
        audioData.PlayButtonClickSound();
        uiData.ExitGame();
        MainCanvas.SetActive(true);
        ScoreCanvas.SetActive(false);
        TangramLevel.SetActive(false);
    }

    public void NextBtnClicked()
    {
        audioData.PlayButtonClickSound();
        levelData.LoadNextLevel(levelData.TangramLevelNo + 1);
    }

    public void PreviousBtnClicked()
    {
        audioData.PlayButtonClickSound();
        levelData.LoadNextLevel(levelData.TangramLevelNo - 1);
    }

    public void ReplayBtnClicked()
    {
        audioData.PlayButtonClickSound();
        movesText.text = "Moves : 0";
    }
}
