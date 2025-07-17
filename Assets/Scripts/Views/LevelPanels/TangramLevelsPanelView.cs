using System.Collections.Generic;
using com.VisionXR.Models;
using UnityEngine;

public class TangramLevelsPanelView : MonoBehaviour
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
    public GameObject TangramScorePanel;
    public GameObject TangramMainPanel;

    [Header(" Local Variables")]
    public int freeLevelCount = 50;
    public int paidLevelCount = 75;

    private void Awake()
    {
        foreach (GameObject g in levelObjects)
        {
            Transform[] allChildren = g.GetComponentsInChildren<Transform>(true);
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
        playerData.UnlockTangramLevelsEvent += SetLocks;
    }

    private void OnDisable()
    {
        playerData.UnlockTangramLevelsEvent -= SetLocks;
    }

    private void SetLevels()
    {
        if (uiData.currentTangramLevelsType == TangramlevelsType.Free)
        {
            for (int i = 0; i < levelObjects.Count; i++)
            {
                levelObjects[i].SetActive(i < freeLevelCount);
            }
        }
        else if (uiData.currentTangramLevelsType == TangramlevelsType.Paid)
        {
            for (int i = 0; i < levelObjects.Count; i++)
            {
                levelObjects[i].SetActive(i < paidLevelCount);
            }
        }
    }

    private void SetLocks()
    {
        if (uiData.currentTangramLevelsType == TangramlevelsType.Free)
        {
            for (int i = 0; i < lockObjects.Count; i++)
            {
                lockObjects[i].SetActive(i >= playerData.tangramFreeLevelsUnlocked);
            }
        }
        else if (uiData.currentTangramLevelsType == TangramlevelsType.Paid)
        {
            if (playerData.isTangramLevelsUnlocked)
            {
                for (int i = 0; i < lockObjects.Count; i++)
                {
                    lockObjects[i].SetActive(i >= playerData.tangramPaidLevelsUnlocked);
                }
            }
            else
            {
                foreach (GameObject lockObj in lockObjects)
                    lockObj.SetActive(true);
            }
        }
    }

    public void LevelBtnClicked(int levelNo)
    {
        if (uiData.currentTangramLevelsType == TangramlevelsType.Free)
        {
            if (levelNo < playerData.tangramFreeLevelsUnlocked)
            {
                audioData.PlayButtonClickSound();
                ScoreCanvasObject.SetActive(true);
                TangramScorePanel.SetActive(true);
                MainCanvasObject.SetActive(false);
                uiData.TangramLevelChanged(levelNo);
            }
        }
        else if (uiData.currentTangramLevelsType == TangramlevelsType.Paid)
        {
            if (playerData.isTangramLevelsUnlocked)
            {
                if (levelNo < playerData.tangramPaidLevelsUnlocked)
                {
                    audioData.PlayButtonClickSound();
                    ScoreCanvasObject.SetActive(true);
                    TangramScorePanel.SetActive(true);
                    MainCanvasObject.SetActive(false);
                    uiData.TangramLevelChanged(levelNo);
                }
            }
            else
            {
                audioData.PlayButtonClickSound();
                playerData.BuyTangramLevels();
            }
        }
    }

    public void BackBtnClicked()
    {
        audioData.PlayButtonClickSound();
        TangramMainPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
