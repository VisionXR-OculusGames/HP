using com.VisionXR.ModelClasses;
using com.VisionXR.Models;
using System;
using System.Collections;
using UnityEngine;

public class MatchStickManager : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public LevelDataSO levelData;
    public LeaderBoardSO leaderBoardData;
    public UIDataSO uiData;
    public InputDataSO inputData;
    public PlayerDataSO playerData;

    [Header("Objects")]
    public GameObject LevelSpawner;
    public MatchStickLevelManager levelManager;
    public ParticleSystem winPS1;
    public ParticleSystem winPS2;

    private void OnEnable()
    {
        levelData.MatchStickLevelSuccesEvent += LevelSuccess;
        levelData.LoadNextLevelEvent += LoadNextLevel;
    }

    private void OnDisable()
    {
        levelData.MatchStickLevelSuccesEvent -= LevelSuccess;
        levelData.LoadNextLevelEvent -= LoadNextLevel;
    }

    private void Start()
    {
        StartCoroutine(HintRoutine());
    }
    private void LevelSuccess(int levelNo)
    {             
        winPS1.Play();
        winPS2.Play();
        leaderBoardData.WriteToMatchStick((playerData.matchStickFreeLevelsUnlocked + playerData.matchStickPaidLevelsUnlocked+playerData.matchStickPaidLevelspack3Unlocked) * 10);
        inputData.DeactivateInput();
    }

    public void LoadNextLevel(int level)
    {
        if (uiData.currentGame == Games.MatchStick)
        {
            levelManager.ChangeLevel(level);
            inputData.ActivateInput();
        }
    }

    private IEnumerator HintRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(10 * 60);
            if(levelData.NoOfHints < 3)
            {
                levelData.NoOfHints++;
            }
        }
    }
}
