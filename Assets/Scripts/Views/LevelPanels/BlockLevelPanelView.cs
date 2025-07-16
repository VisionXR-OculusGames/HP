using com.VisionXR.Models;
using System.Collections.Generic;
using UnityEngine;

public class BlockLevelsPanelView : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public AudioDataSO audioData;
    public UIDataSO uiData;
    public PlayerDataSO playerData;

    [Header(" Level Objects")]
    public List<GameObject> levelObjects;
    public List<GameObject> lockObjects;

    [Header(" Local Objects")]
    public GameObject MainCanvasObject;
    public GameObject ScoreCanvasObject;
    public GameObject BlockScorePanel;
    public GameObject BlocksMainPanel;

    [Header(" Local Variables")]
    public int freeLevelCount = 25;
    public int paidLevelCount = 100;
    public int paidLevelPackk3Count = 100;


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
        playerData.UnlockSlideTheBlockLevelsEvent += SetLocks;
    }

    private void OnDisable()
    {
        playerData.UnlockSlideTheBlockLevelsEvent -= SetLocks;
    }
    private void SetLevels()
    {
        if (uiData.currentBlocksLevelsType == SlideTheBlocklevelsType.Free)
        {
            for (int i = 0; i < levelObjects.Count; i++)
            {
                if (i < freeLevelCount)
                {
                    levelObjects[i].SetActive(true);
                }
                else
                {
                    levelObjects[i].SetActive(false);
                }
            }
        }
        else if(uiData.currentBlocksLevelsType == SlideTheBlocklevelsType.Paid)
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

        else if (uiData.currentBlocksLevelsType == SlideTheBlocklevelsType.Pack3)
        {
            for (int i = 0; i < levelObjects.Count; i++)
            {
                if (i < paidLevelPackk3Count)
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
        if (uiData.currentBlocksLevelsType == SlideTheBlocklevelsType.Free)
        {
            for (int i = 0; i < lockObjects.Count; i++)
            {
                if (i < playerData.slideTheBlockFreeLevelsUnlocked)
                {
                    lockObjects[i].SetActive(false);
                }
                else
                {
                    lockObjects[i].SetActive(true);
                }
            }
        }
        else if (uiData.currentBlocksLevelsType == SlideTheBlocklevelsType.Paid)
        {
            if (playerData.isSlideTheBlockLevelsUnlocked)
            {
                for (int i = 0; i < lockObjects.Count; i++)
                {
                    if (i < playerData.slideTheBlockPaidLevelsUnlocked)
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

        else if (uiData.currentBlocksLevelsType == SlideTheBlocklevelsType.Pack3)
        {
            if (playerData.isSlideTheBlockLevelsUnlocked)
            {
                for (int i = 0; i < lockObjects.Count; i++)
                {
                    if (i < playerData.slideTheBlockPaidLevelsPack3Unlocked)
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
        if (uiData.currentBlocksLevelsType == SlideTheBlocklevelsType.Free)
        {
            if (levelNo < playerData.slideTheBlockFreeLevelsUnlocked)
            {
                audioData.PlayButtonClickSound();
                ScoreCanvasObject.SetActive(true);
                BlockScorePanel.SetActive(true);
                MainCanvasObject.SetActive(false);
                uiData.BlockLevelChanged(levelNo);
            }
        }
        else if(uiData.currentBlocksLevelsType == SlideTheBlocklevelsType.Paid)
        {
            if (playerData.isSlideTheBlockLevelsUnlocked)
            {
                if (levelNo < playerData.slideTheBlockPaidLevelsUnlocked)
                {
                    audioData.PlayButtonClickSound();
                    ScoreCanvasObject.SetActive(true);
                    BlockScorePanel.SetActive(true);
                    MainCanvasObject.SetActive(false);
                    uiData.BlockLevelChanged(levelNo);
                }
            }
            else
            {
                audioData.PlayButtonClickSound();
                playerData.BuySlideTheBlockLevels();
            }
        }
        else if (uiData.currentBlocksLevelsType == SlideTheBlocklevelsType.Pack3)
        {
            if (playerData.isSlideTheBlockLevelsUnlocked)
            {
                if (levelNo < playerData.slideTheBlockPaidLevelsPack3Unlocked)
                {
                    audioData.PlayButtonClickSound();
                    ScoreCanvasObject.SetActive(true);
                    BlockScorePanel.SetActive(true);
                    MainCanvasObject.SetActive(false);
                    uiData.BlockLevelChanged(levelNo);
                }
            }
            else
            {
                audioData.PlayButtonClickSound();
                playerData.BuySlideTheBlockLevels();
            }
        }
    }

    public void BackBtnClicked()
    {
        audioData.PlayButtonClickSound();
        BlocksMainPanel.SetActive(true);
        gameObject.SetActive(false);
    }

}
