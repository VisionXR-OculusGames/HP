using com.VisionXR.Models;
using TMPro;
using UnityEngine;

public class MatchStickMainPanel : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public UIDataSO uiData;
    public AudioDataSO audioData;
    public PlayerDataSO playerData;

    [Header(" Panel Objects")]
    public GameObject Gamespanel;
    public GameObject MatchSticLevelsPanels;

    [Header(" Text Objects")]
    public TMP_Text pack1Text;
    public TMP_Text pack2Text;
    public TMP_Text pack3Text;

    [Header(" Local Variables")]
    public int freeLevelCount = 50;
    public int paidLevelCount = 75;
    public int paidLevelPack3Count = 75;

    private void OnEnable()
    {
        pack1Text.text = "Pack1 (" + playerData.matchStickFreeLevelsUnlocked + "/" + freeLevelCount+")";
        pack2Text.text = "Pack2 (" + playerData.matchStickPaidLevelsUnlocked + "/" + paidLevelCount+")";
        pack3Text.text = "Pack3 (" + playerData.matchStickPaidLevelsUnlocked + "/" + paidLevelPack3Count+")";
    }

    public void FreeLevelsButtonClicked()
    {
        audioData.PlayButtonClickSound();
        uiData.currentMatchStickLevelsType = MatchSticklevelsType.Free;
        MatchSticLevelsPanels.SetActive(true);
        gameObject.SetActive(false);
    }

    public void PaidLevelsButtonClicked()
    {
        audioData.PlayButtonClickSound();
        uiData.currentMatchStickLevelsType = MatchSticklevelsType.Paid;
        MatchSticLevelsPanels.SetActive(true);
        gameObject.SetActive(false);
    }

    public void PaidLevelsPack3ButtonClicked()
    {
        audioData.PlayButtonClickSound();
        uiData.currentMatchStickLevelsType = MatchSticklevelsType.Pack3;
        MatchSticLevelsPanels.SetActive(true);
        gameObject.SetActive(false);
    }

    public void BackBtnClicked()
    {
        audioData.PlayButtonClickSound();
        Gamespanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
