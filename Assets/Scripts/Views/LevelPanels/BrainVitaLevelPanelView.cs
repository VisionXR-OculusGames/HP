using com.VisionXR.Models;
using System.Collections.Generic;
using UnityEngine;

public class BrainVitaLevelPanelView: MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public AudioDataSO audioData;
    public UIDataSO uiData;
    public PlayerDataSO playerData;

    [Header(" Local Objects")]
    public GameObject MainCanvasObject;
    public GameObject ScoreCanvasObject;
    public GameObject BrainVitaScorePanel;
    public GameObject BrainvitaMainPanel;

    [Header(" Level Buttons")]
    public List<GameObject> freeLevelButtons;
    public List<GameObject> paidLevelButtons;
    public List<GameObject> lockObjects;

    private void Awake()
    {
        foreach (GameObject g in paidLevelButtons)
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
        SetButtons();
        SetLocks();
        playerData.UnlockBrainVitaLevelsEvent += SetLocks;
    }

    private void OnDisable()
    {
        playerData.UnlockBrainVitaLevelsEvent -= SetLocks;
    }

    private void SetButtons()
    {
        foreach (var b in freeLevelButtons)
        {
            b.SetActive(false);
        }

        foreach (var b in paidLevelButtons)
        {
            b.SetActive(false);
        }

        if (uiData.currentBrainVitalevelsType == BrainVitalevelsType.Free)
        {
            foreach (var b in freeLevelButtons)
            {
                b.SetActive(true);
            }
        }
        else
        {
            foreach (var b in paidLevelButtons)
            {
                b.SetActive(true);
            }
        }

       
    }

    private void SetLocks()
    {
        if (uiData.currentBrainVitalevelsType == BrainVitalevelsType.Paid)
        {
            if (!playerData.isBrainVitaLevelsUnlocked)
            {
                for (int i = 0; i < lockObjects.Count; i++)
                {
                    lockObjects[i].SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < lockObjects.Count; i++)
                {
                    lockObjects[i].SetActive(false);
                }
            }
        }
    }
    public void LevelBtnClicked(int levelNo)
    {
        if (uiData.currentBrainVitalevelsType == BrainVitalevelsType.Free)
        {
            audioData.PlayButtonClickSound();

            ScoreCanvasObject.SetActive(true);
            BrainVitaScorePanel.SetActive(true);
            MainCanvasObject.SetActive(false);

            uiData.BrainvitaLevelChanged(levelNo);
        }
        else
        {
            if (playerData.isBrainVitaLevelsUnlocked)
            {
                audioData.PlayButtonClickSound();

                ScoreCanvasObject.SetActive(true);
                BrainVitaScorePanel.SetActive(true);
                MainCanvasObject.SetActive(false);

                uiData.BrainvitaLevelChanged(levelNo);
            }
            else
            {
                audioData.PlayButtonClickSound();
                playerData.BuyBrainVitaLevels();
            }
        }
    }


    public void BackBtnClciked()
    {
        audioData.PlayButtonClickSound();
        BrainvitaMainPanel.SetActive(true);
        gameObject.SetActive(false);
    }

}
