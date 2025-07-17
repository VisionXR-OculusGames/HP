using com.VisionXR.ModelClasses;
using com.VisionXR.Models;
using UnityEngine;

public class TangramPuzzleManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public LevelDataSO levelData;
    public InputDataSO inputData;
    public UIDataSO uiData;
    public PlayerDataSO playerData;
    public LeaderBoardSO leaderBoardData;
    public ParticleSystem winPS1, winPS2;

    [Header("External References")]
    public TangramPuzzleLevelManager levelManager;

    private void OnEnable()
    {
        levelData.TangramLevelSuccesEvent += LevelSuccess;
        levelData.LoadNextLevelEvent += LoadNextLevel;
    }

    private void OnDisable()
    {
        levelData.TangramLevelSuccesEvent -= LevelSuccess;
        levelData.LoadNextLevelEvent -= LoadNextLevel;
    }

    private void LevelSuccess(int levelNo)
    {
        winPS1.Play();
        winPS2.Play();
        leaderBoardData.WriteToTangram((playerData.tangramFreeLevelsUnlocked + playerData.tangramPaidLevelsUnlocked) * 10);
        inputData.DeactivateInput();
    }

    public void LoadNextLevel(int level)
    {
        if (uiData.currentGame == Games.Tangram)
        {
            levelManager.ChangeLevel(level);
        }
    }
}
