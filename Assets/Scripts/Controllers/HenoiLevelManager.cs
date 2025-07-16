using com.VisionXR.Models;
using UnityEngine;

public class HenoiLevelManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public LevelDataSO levelData;
    public UIDataSO uiData;
    public InputDataSO inputData;
    public PlayerDataSO playerData;

    [Header("Local Objects")]
    public GameObject LevelPlacer;
    public string Path = "Levels/Level";
    private GameObject currentLevelObject;
    private void OnEnable()
    {
        uiData.HenoiLevelChangedEvent += ChangeLevel;
        uiData.ExitGameEvent += DestroyLevel;
        
    }

    private void OnDisable()
    {
        uiData.HenoiLevelChangedEvent -= ChangeLevel;
        uiData.ExitGameEvent -= DestroyLevel;

    }

    public void ChangeLevel(int levelNo)
    {
        if(uiData.currentGame != Games.TowersOfHenoi)
        {
            return;
        }

        levelData.SetHenoiLevel(levelNo);   
        loadLevel(levelNo);

        if (playerData.henoiFreeLevelsUnlocked < levelNo + 1)
        {
            playerData.henoiFreeLevelsUnlocked = levelNo + 1;
        }

        playerData.SaveLevelsData();
    }

    public void ReplayLevel()
    {
        loadLevel(levelData.HenoiLevelNo);
    }
    public void loadLevel(int levelNumber)
    {

        // Destroy if level exists
        DestroyLevel();

       

        // Construct the path string
        string levelPath = Path + levelNumber.ToString();

        Debug.Log(levelPath);

        // Load the prefab from the constructed path
        GameObject levelPrefab = Resources.Load<GameObject>(levelPath);

        if (levelPrefab != null)
        {
            // Instantiate the level prefab under the levelParent transform
            currentLevelObject = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);

             currentLevelObject.transform.position = LevelPlacer.transform.position;
             currentLevelObject.transform.rotation = LevelPlacer.transform.rotation;
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
