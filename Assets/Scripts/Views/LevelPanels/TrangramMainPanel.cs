using com.VisionXR.Models;
using UnityEngine;

public class TrangramMainPanel : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public UIDataSO uiData;
    public AudioDataSO audioData;
    public PlayerDataSO playerData;

    [Header(" Panel Objects")]
    public GameObject Gamespanel;
    public GameObject TangramLevelPanel;
    public void FreeLevelsButtonClicked()
    {
        audioData.PlayButtonClickSound();
        uiData.currentTangramLevelsType = TangramlevelsType.Free;
        TangramLevelPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void BackBtnClicked()
    {
        audioData.PlayButtonClickSound();
        Gamespanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
