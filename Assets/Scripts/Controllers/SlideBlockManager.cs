using com.VisionXR.ModelClasses;
using com.VisionXR.Models;
using System.Collections;
using UnityEngine;

public class SlideBlockManager : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public LevelDataSO levelData;
    public InputDataSO inputData;
    public UIDataSO uiData;
    public PlayerDataSO playerData;
    public LeaderBoardSO leaderBoardData;

    [Header("Objects")]
    public GameObject LevelPlacer;
    public SlideBlockLevelManager levelManager;
    public ParticleSystem winPS1;
    public ParticleSystem winPS2;



    private void OnEnable()
    {
        levelData.BlockLevelSuccesEvent += LevelSuccess;
        levelData.LoadNextLevelEvent += LoadNextLevel;
    }

    private void OnDisable()
    {
        levelData.BlockLevelSuccesEvent -= LevelSuccess;
        levelData.LoadNextLevelEvent -= LoadNextLevel;
    }

    private void LevelSuccess(int levelNo)
    {
        winPS1.Play();
        winPS2.Play();     
        leaderBoardData.WriteToSlideTheBlock((playerData.slideTheBlockFreeLevelsUnlocked+playerData.slideTheBlockPaidLevelsUnlocked+playerData.matchStickPaidLevelspack3Unlocked) * 10);
        inputData.DeactivateInput();      
    }

    public void LoadNextLevel(int level)
    {
        if (uiData.currentGame == Games.SlideTheBlock)
        {
            levelManager.ChangeLevel(level);
        }
    }

  
}
