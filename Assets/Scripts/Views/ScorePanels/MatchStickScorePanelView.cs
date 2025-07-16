using com.VisionXR.Models;
using TMPro;
using UnityEngine;

public class MatchStickScorePanelView : MonoBehaviour
{
    [Header(" Scriptable Object")]
    public UIDataSO uiData;
    public AudioDataSO audioData;
    public LevelDataSO levelData;

    [Header(" Canvas Object")]
    public TMP_Text levelText;
    public TMP_Text movesText;
    public TMP_Text descriptiontext;
    public TMP_Text hintText;
    public TMP_Text hintNumberText;



    private void OnEnable()
    {
       
        levelData.MatchStickLevelSetEvent += ShowLevel;
        levelData.SetMatchStickMovesEvent += ShowMoves;

        hintNumberText.text = levelData.NoOfHints.ToString();
    }

    private void OnDisable()
    {
        levelData.MatchStickLevelSetEvent -= ShowLevel;
        levelData.SetMatchStickMovesEvent -= ShowMoves;
    }

    private void ShowLevel()
    {
        levelText.text = " Level : " + (levelData.MatchStickLevelNo+1).ToString();
        hintText.gameObject.SetActive(false);
    }

    private void ShowMoves(int currentMove,int TotalMoves,string hint)
    {
        movesText.text = " Moves : "+currentMove+"/"+TotalMoves;
        if(TotalMoves == 1)
        {
            descriptiontext.text = "Move 1 stick to solve the equation";
        }
        else
        {
            descriptiontext.text = "Move 2 sticks to solve the equation";
        }
        hintText.text = hint;
    }

    public void ReplayBtnClicked()
    {
        audioData.PlayButtonClickSound();
        movesText.text = " Moves : " + 0 + "/" + 0;
    }
   
    public void HintBtnClicked()
    {
        audioData.PlayButtonClickSound();
        if(levelData.NoOfHints > 0 && !hintText.gameObject.activeSelf)
        {
            levelData.NoOfHints--;
            hintText.gameObject.SetActive(true);
            hintNumberText.text = levelData.NoOfHints.ToString();
        }
    }
}
