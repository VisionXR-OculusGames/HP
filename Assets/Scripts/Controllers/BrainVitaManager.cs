using com.VisionXR.ModelClasses;
using com.VisionXR.Models;
using System;
using UnityEngine;

public class BrainVitaManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public UIDataSO uiData;
    public InputDataSO inputData;
    public LevelDataSO levelData;
    public PlayerDataSO playerData;
    public LeaderBoardSO leaderBoardData;

    [Header("Local Objects")]
    public GameObject levelSpawner;
    private GameObject currentLevelObject;
    public string FreelevelsPath = "Levels/Level";
    public string PaidlevelsPath = "Levels/Level";
    public ParticleSystem winPs1;
    public ParticleSystem winPs2;
   

    private void OnEnable()
    {
        uiData.BrainVitaLevelChangedEvent += StartGame;
        uiData.ExitGameEvent += DestroyLevel;

        levelData.LoadNextLevelEvent += LoadNextLevel;
        levelData.BraivitaLevelSuccesEvent += LevelSuccess;
    }

    private void OnDisable()
    {
        uiData.BrainVitaLevelChangedEvent -= StartGame;
        uiData.ExitGameEvent -= DestroyLevel;

        levelData.LoadNextLevelEvent -= LoadNextLevel;
        levelData.BraivitaLevelSuccesEvent -= LevelSuccess;
    }

    private void LoadNextLevel(int levelNo)
    {
        if (uiData.currentGame == Games.BrainVita)
        {
            StartGame(levelNo + 1);
            inputData.ActivateInput();

            if (uiData.currentBrainVitalevelsType == BrainVitalevelsType.Free)
            {
                if (playerData.brainvitaFreeLevelsUnlocked < levelNo + 1)
                {
                    playerData.brainvitaFreeLevelsUnlocked = levelNo + 1;
                }
            }
            else
            {
                if (playerData.brainvitaPaidLevelsUnlocked < levelNo + 1)
                {
                    playerData.brainvitaPaidLevelsUnlocked = levelNo + 1;
                }
            }

            playerData.SaveLevelsData();
        }
    }

    private void LevelSuccess(int levelNo)
    {
        winPs1.Play();
        winPs2.Play();
        inputData.DeactivateInput();
    }

    public void ReplayBtnClicked()
    {
        StartGame(levelData.BrainVitaLevelNo);
    }

    public void DestroyLevel()
    {
        if(currentLevelObject != null)
        {
            Destroy(currentLevelObject);
            currentLevelObject = null;
        }
    }
    public void StartGame(int levelNumber)
    {
        // Destroy if level exists
        DestroyLevel();

        levelData.SetBrainVitaLevel(levelNumber);

        // Construct the path string
        // Construct the path string
        string levelPath;
        if (uiData.currentBrainVitalevelsType == BrainVitalevelsType.Free)
        {
            levelPath = FreelevelsPath + levelNumber.ToString();
        }
        else
        {
            levelPath = PaidlevelsPath + levelNumber.ToString();
        }

        Debug.Log(levelPath);

        // Load the prefab from the constructed path
        GameObject levelPrefab = Resources.Load<GameObject>(levelPath);

        if (levelPrefab != null)
        {
            // Instantiate the level prefab under the levelParent transform
            currentLevelObject = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);

            currentLevelObject.transform.position = levelSpawner.transform.position;
            currentLevelObject.transform.rotation = levelSpawner.transform.rotation;
            currentLevelObject.transform.parent = levelSpawner.transform;
            currentLevelObject.transform.localEulerAngles = new Vector3(-90, 0, 0);
            inputData.ActivateInput();
           

        }
        else
        {
            Debug.LogWarning("Level " + levelNumber + " prefab not found!");
            uiData.AllLevelsComplete();
        }

    }
}
