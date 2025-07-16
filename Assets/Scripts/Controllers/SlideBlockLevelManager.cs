using com.VisionXR.Models;
using UnityEngine;

public class SlideBlockLevelManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public PlayerDataSO playerData;
    public LevelDataSO levelData;
    public UIDataSO uiData;
    public InputDataSO inputData;

    [Header("Local Objects")]
    public GameObject LevelPlacer;
    public float scaleFactor;
    public string FreelevelsPath = "Levels/Level";
    public string PaidlevelsPath = "Levels/Level";
    public string PaidlevelsPack3Path = "Levels/Level";
    private GameObject currentLevelObject;

    private void OnEnable()
    {
        uiData.BlockLevelChangedEvent += ChangeLevel;
        uiData.ExitGameEvent += DestroyLevel;
        
    }

    private void OnDisable()
    {
        uiData.BlockLevelChangedEvent -= ChangeLevel;
        uiData.ExitGameEvent -= DestroyLevel;

    }

    public void ChangeLevel(int levelNo)
    {
        levelData.SetBlockLevel(levelNo);

        loadLevel(levelNo);

        if (uiData.currentBlocksLevelsType == SlideTheBlocklevelsType.Free)
        {
            if (playerData.slideTheBlockFreeLevelsUnlocked < levelNo + 1)
            {
                playerData.slideTheBlockFreeLevelsUnlocked = levelNo + 1;
            }
        }
        else if (uiData.currentBlocksLevelsType == SlideTheBlocklevelsType.Paid)
        {
            if (playerData.slideTheBlockPaidLevelsUnlocked < levelNo + 1)
            {
                playerData.slideTheBlockPaidLevelsUnlocked = levelNo + 1;
            }
        }
        else if (uiData.currentBlocksLevelsType == SlideTheBlocklevelsType.Pack3)
        {
            if (playerData.slideTheBlockPaidLevelsPack3Unlocked < levelNo + 1)
            {
                playerData.slideTheBlockPaidLevelsPack3Unlocked = levelNo + 1;
            }
        }

        playerData.SaveLevelsData();
    }

    public void ReplayLevel()
    {
        loadLevel(levelData.BlockLevelNo);
    }

    public void loadLevel(int levelNumber)
    {

        // Destroy if level exists
        DestroyLevel();

      
        // Construct the path string
        string levelPath;
        if (uiData.currentBlocksLevelsType == SlideTheBlocklevelsType.Free)
        {
            levelPath = FreelevelsPath + levelNumber.ToString();
        }
        else if(uiData.currentBlocksLevelsType == SlideTheBlocklevelsType.Paid)
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

            currentLevelObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            currentLevelObject.transform.parent = LevelPlacer.transform;

            inputData.ActivateInput();

            Debug.Log("Level loaded" );

        }
        else
        {
            Debug.Log("Level " + levelNumber + " prefab not found!");
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
