using com.VisionXR.Models;
using System.Collections;
using UnityEngine;

public class HenoiManager : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public LevelDataSO levelData;
    public InputDataSO inputData;
    public UIDataSO uiData;

    [Header("Objects")]
    public GameObject LevelPlacer;
    public HenoiLevelManager levelManager;
    public ParticleSystem winPS1;
    public ParticleSystem winPS2;

    private void OnEnable()
    {
        levelData.HenoiLevelSuccesEvent += LevelSuccess;
        levelData.LoadNextLevelEvent += LoadNextLevel;
    }

    private void OnDisable()
    {
        levelData.HenoiLevelSuccesEvent -= LevelSuccess;
        levelData.LoadNextLevelEvent -= LoadNextLevel;
    }

    private void LevelSuccess(int levelNo)
    {
         winPS1.Play();
         winPS2.Play();
        inputData.DeactivateInput();
    }

    public void LoadNextLevel(int level)
    {
        if (uiData.currentGame == Games.TowersOfHenoi)
        {
            levelManager.ChangeLevel(level);
        }
    }

}
