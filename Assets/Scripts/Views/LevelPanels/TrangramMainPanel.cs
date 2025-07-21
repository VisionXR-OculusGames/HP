using com.VisionXR.Models;
using TMPro;
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

    [Header(" Text Objects")]
    public TMP_Text pack1Text;
    public TMP_Text pack2Text;

    [Header(" Local Variables")]
    public int freeLevelCount = 50;
    public int paidLevelCount = 75;

    private void OnEnable()
    {
        pack1Text.text = "Pack1 (" + playerData.tangramFreeLevelsUnlocked + "/" + freeLevelCount + ")";
        pack2Text.text = "Pack2 (" + playerData.tangramPaidLevelsUnlocked + "/" + paidLevelCount + ")";
    }
    public void FreeLevelsButtonClicked()
    {
        audioData.PlayButtonClickSound();
        uiData.currentTangramLevelsType = TangramlevelsType.Free;
        TangramLevelPanel.SetActive(true);
        gameObject.SetActive(false);
    }
    public void PaidLevelsButtonClicked()
    {
        audioData.PlayButtonClickSound();
        uiData.currentTangramLevelsType = TangramlevelsType.Paid;
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
