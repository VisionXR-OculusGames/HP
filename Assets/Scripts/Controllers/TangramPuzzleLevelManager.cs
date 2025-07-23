using com.VisionXR.Models;
using UnityEngine;

public class TangramPuzzleLevelManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public PlayerDataSO playerData;
    public LevelDataSO levelData;
    public UIDataSO uiData;
    public InputDataSO inputData;

    [Header("Local Objects")]
    public GameObject LevelPlacer;
    public float scaleFactor = 0.3f;
    public string FreelevelsPath = "Tangram/Pack1/Level";
    public string PaidlevelsPath = "Tangram/Pack2/Level";
    private GameObject currentLevelObject;

    private void OnEnable()
    {
        uiData.TangramLevelChangedEvent += ChangeLevel;
        uiData.ExitGameEvent += DestroyLevel;
    }

    private void OnDisable()
    {
        uiData.TangramLevelChangedEvent -= ChangeLevel;
        uiData.ExitGameEvent -= DestroyLevel;
    }

    public void ChangeLevel(int levelNo)
    {
        levelData.SetTangramLevel(levelNo);

        LoadLevel(levelNo);

        if (uiData.currentTangramLevelsType == TangramlevelsType.Free)
        {
            if (playerData.tangramFreeLevelsUnlocked < levelNo + 1)
            {
                playerData.tangramFreeLevelsUnlocked = levelNo + 1;
            }
        }
        else
        {
            if (playerData.tangramPaidLevelsUnlocked < levelNo + 1)
            {
                playerData.tangramPaidLevelsUnlocked = levelNo + 1;
            }
        }

        playerData.SaveLevelsData();
    }

    public void ReplayLevel()
    {
        LoadLevel(levelData.TangramLevelNo);
    }

    public void LoadLevel(int levelNumber)
    {
        DestroyLevel();

        string levelPath;
        if (uiData.currentTangramLevelsType == TangramlevelsType.Free)
        {
            levelPath = FreelevelsPath + levelNumber.ToString();
        }
        else
        {
            levelPath = PaidlevelsPath + levelNumber.ToString();
        }

        GameObject levelPrefab = Resources.Load<GameObject>(levelPath);

        if (levelPrefab != null)
        {
            currentLevelObject = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);
            currentLevelObject.transform.position = LevelPlacer.transform.position + new Vector3(0,0.15f,0);
            currentLevelObject.transform.rotation = LevelPlacer.transform.rotation;
            currentLevelObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            currentLevelObject.transform.parent = LevelPlacer.transform;

            inputData.ActivateInput();

            Debug.Log("Tangram level loaded");
        }
        else
        {
            Debug.Log("Tangram Level " + levelNumber + " prefab not found!");
            uiData.AllLevelsComplete();
        }
    }

    public void DestroyLevel()
    {
        if (currentLevelObject != null)
        {
            Destroy(currentLevelObject);
        }
    }
}