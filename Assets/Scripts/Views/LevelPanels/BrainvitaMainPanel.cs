using com.VisionXR.Models;
using UnityEngine;

public class BrainvitaMainPanel : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public UIDataSO uiData;
    public AudioDataSO audioData;

    [Header(" Panel Objects")]
    public GameObject Gamespanel;
    public GameObject BrainvitaLevelsPanels;

    public void FreeLevelsButtonClicked()
    {
        audioData.PlayButtonClickSound();
        uiData.currentBrainVitalevelsType = BrainVitalevelsType.Free;
        BrainvitaLevelsPanels.SetActive(true);
        gameObject.SetActive(false);
    }

    public void PaidLevelsButtonClicked()
    {
        audioData.PlayButtonClickSound();
        uiData.currentBrainVitalevelsType = BrainVitalevelsType.Paid;
        BrainvitaLevelsPanels.SetActive(true);
        gameObject.SetActive(false);
    }

    public void BackBtnClicked()
    {
        audioData.PlayButtonClickSound();
        Gamespanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
