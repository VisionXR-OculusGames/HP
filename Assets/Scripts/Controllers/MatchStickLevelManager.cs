using com.VisionXR.Models;
using UnityEngine;

public class MatchStickLevelManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public UIDataSO uiData;
    public LevelDataSO levelData;
    public InputDataSO inputData;
    public PlayerDataSO playerData;

    [Header("Local Objects")]
    public GameObject LevelPlacer;
    public string FreelevelsPath = "Levels/Level";
    public string PaidlevelsPath = "Levels/Level";
    public string PaidlevelsPack3Path = "Levels/Level";
    private GameObject currentLevelObject;
    private void OnEnable()
    {
        uiData.MatchStickLevelChangedEvent += ChangeLevel;
        uiData.ExitGameEvent += DestroyLevel;
        
    }

    private void OnDisable()
    {
        uiData.MatchStickLevelChangedEvent -= ChangeLevel;
        uiData.ExitGameEvent -= DestroyLevel;

    }

    public void ChangeLevel(int levelNo)
    {
        
        levelData.SetMatchStickLevel(levelNo);   
        loadLevel(levelNo);

        if (uiData.currentMatchStickLevelsType == MatchSticklevelsType.Free)
        {
            if (playerData.matchStickFreeLevelsUnlocked < levelNo + 1)
            {
                playerData.matchStickFreeLevelsUnlocked = levelNo + 1;
            }
        }
        else if (uiData.currentMatchStickLevelsType == MatchSticklevelsType.Paid)
        {
            if (playerData.matchStickPaidLevelsUnlocked < levelNo + 1)
            {
                playerData.matchStickPaidLevelsUnlocked = levelNo + 1;
            }
        }
        else if (uiData.currentMatchStickLevelsType == MatchSticklevelsType.Pack3)
        {
            if (playerData.matchStickPaidLevelspack3Unlocked < levelNo + 1)
            {
                playerData.matchStickPaidLevelspack3Unlocked = levelNo + 1;      
            }
        }

        playerData.SaveLevelsData();
    }

    public void ReplayBtnClicked()
    {
        loadLevel(levelData.MatchStickLevelNo);
    }

    public void loadLevel(int levelNumber)
    {

        // Destroy if level exists
        DestroyLevel();


        // Construct the path string
        string levelPath;
        if (uiData.currentMatchStickLevelsType == MatchSticklevelsType.Free)
        {
             levelPath = FreelevelsPath + levelNumber.ToString();
        }
        else if (uiData.currentMatchStickLevelsType == MatchSticklevelsType.Paid)
        {
            levelPath = PaidlevelsPath + levelNumber.ToString();
        }
        else 
        {
            levelPath = PaidlevelsPack3Path + levelNumber.ToString();
        }


        // Load the prefab from the constructed path
        GameObject levelPrefab = Resources.Load<GameObject>(levelPath);

        if (levelPrefab != null)
        {
            // Instantiate the level prefab under the levelParent transform
            currentLevelObject = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);

             currentLevelObject.transform.position = LevelPlacer.transform.position;
             currentLevelObject.transform.rotation = LevelPlacer.transform.rotation;
             currentLevelObject.transform.parent = LevelPlacer.transform;

            Debug.LogWarning("Level loaded" );
            inputData.ActivateInput();

        }
        else
        {
            Debug.LogWarning("Level " + levelNumber + " prefab not found!");
            uiData.AllLevelsComplete();
        }
    }

    // Call this function to Destroy Current level
    public void DestroyLevel()
    {
        if (currentLevelObject != null)
        {
            Destroy(currentLevelObject);
            
        }
    }
}
    