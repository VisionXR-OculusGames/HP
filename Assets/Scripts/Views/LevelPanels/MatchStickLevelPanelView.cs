using com.VisionXR.Models;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class MatchStickLevelPanelView : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public PlayerDataSO playerData;
    public AudioDataSO audioData;
    public UIDataSO uiData;

    [Header(" Level Objects")]
    public List<GameObject> levelObjects;
    public List<GameObject> lockObjects;

    [Header(" Local Objects")]
    public GameObject MainCanvasObject;
    public GameObject GamesPanel;

    public GameObject ScoreCanvas;
    public GameObject MatchStickPanel;

    [Header(" Local Variables")]
    public int freeLevelCount = 50;
    public int paidLevelCount = 75;
    public int paidLevelPack3Count = 75;

    private void Awake()
    {
        foreach (GameObject g in levelObjects)
        {
            Transform[] allChildren = g.GetComponentsInChildren<Transform>(true); // include inactive
            foreach (Transform t in allChildren)
            {
                if (t.name == "Lock")
                {
                    lockObjects.Add(t.gameObject);
                    break;
                }
            }
        }
    }

    private void OnEnable()
    {
        SetLevels();
        SetLocks();

        playerData.UnlockMatchStickLevelsEvent += SetLocks;
    }

    private void OnDisable()
    {
        playerData.UnlockMatchStickLevelsEvent -= SetLocks;
    }
    private void SetLevels()
    {
        if (uiData.currentMatchStickLevelsType == MatchSticklevelsType.Free)
        {
            for (int i = 0; i < levelObjects.Count; i++)
            {
                if(i < freeLevelCount)
                {
                    levelObjects[i].SetActive(true);
                }
                else
                {
                    levelObjects[i].SetActive(false);
                }
            }
        }
        else if (uiData.currentMatchStickLevelsType == MatchSticklevelsType.Paid)
        {
            for (int i = 0; i < levelObjects.Count; i++)
            {
                if (i < paidLevelCount)
                {
                    levelObjects[i].SetActive(true);
                }
                else
                {
                    levelObjects[i].SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < levelObjects.Count; i++)
            {
                if (i < paidLevelPack3Count)
                {
                    levelObjects[i].SetActive(true);
                }
                else
                {
                    levelObjects[i].SetActive(false);
                }
            }
        }
    }

    private void SetLocks()
    {
        if (uiData.currentMatchStickLevelsType == MatchSticklevelsType.Free)
        {
            for (int i = 0; i < lockObjects.Count; i++)
            {
                if (i < playerData.matchStickFreeLevelsUnlocked)
                {
                    lockObjects[i].SetActive(false);
                }
                else
                {
                    lockObjects[i].SetActive(true);
                }
            }
        }
        else if (uiData.currentMatchStickLevelsType == MatchSticklevelsType.Paid)
        {
            if (playerData.isMatchSticksLevelsUnlocked)
            {
                for (int i = 0; i < lockObjects.Count; i++)
                {
                    if (i < playerData.matchStickPaidLevelsUnlocked)
                    {
                        lockObjects[i].SetActive(false);
                    }
                    else
                    {
                        lockObjects[i].SetActive(true);
                    }
                }
            }
            else
            {
                for (int i = 0; i < lockObjects.Count; i++)
                {

                    lockObjects[i].SetActive(true);

                }
            }
        }
        else if (uiData.currentMatchStickLevelsType == MatchSticklevelsType.Pack3)
        {
            if (playerData.isMatchSticksLevelsUnlocked)
            {
                for (int i = 0; i < lockObjects.Count; i++)
                {
                    if (i < playerData.matchStickPaidLevelspack3Unlocked)
                    {
                        lockObjects[i].SetActive(false);
                    }
                    else
                    {
                        lockObjects[i].SetActive(true);
                    }
                }
            }
            else
            {
                for (int i = 0; i < lockObjects.Count; i++)
                {

                    lockObjects[i].SetActive(true);

                }
            }
        }
    }

    
    public void LevelBtnClicked(int levelNo)
    {
        if (uiData.currentMatchStickLevelsType == MatchSticklevelsType.Free)
        {
            if(levelNo < playerData.matchStickFreeLevelsUnlocked)
            {
                audioData.PlayButtonClickSound();

                ScoreCanvas.SetActive(true);
                MatchStickPanel.SetActive(true);


                uiData.MatchStickLevelChanged(levelNo);

                MainCanvasObject.SetActive(false);
            }
        }
        else if (uiData.currentMatchStickLevelsType == MatchSticklevelsType.Paid)
        {
            if (playerData.isMatchSticksLevelsUnlocked)
            {
                if (levelNo < playerData.matchStickPaidLevelsUnlocked)
                {
                    audioData.PlayButtonClickSound();

                    ScoreCanvas.SetActive(true);
                    MatchStickPanel.SetActive(true);


                    uiData.MatchStickLevelChanged(levelNo);

                    MainCanvasObject.SetActive(false);
                }

            }
            else
            {
                audioData.PlayButtonClickSound();
                playerData.BuyMatchStickLevels();
            }
        }

        else if (uiData.currentMatchStickLevelsType == MatchSticklevelsType.Pack3)
        {
            if (playerData.isMatchSticksLevelsUnlocked)
            {
                if (levelNo < playerData.matchStickPaidLevelspack3Unlocked)
                {
                    audioData.PlayButtonClickSound();

                    ScoreCanvas.SetActive(true);
                    MatchStickPanel.SetActive(true);


                    uiData.MatchStickLevelChanged(levelNo);

                    MainCanvasObject.SetActive(false);
                }

            }
            else
            {
                audioData.PlayButtonClickSound();
                playerData.BuyMatchStickLevels();
            }
        }
    }

    public void BackBtnClicked()
    {
        audioData.PlayButtonClickSound();
        GamesPanel.SetActive(true);
        gameObject.SetActive(false);
    }

}
