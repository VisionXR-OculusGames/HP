using com.VisionXR.Models;
using TMPro;
using UnityEngine;

public class BlockMainPanel : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public UIDataSO uiData;
    public AudioDataSO audioData;
    public PlayerDataSO playerData;

    [Header(" Panel Objects")]
    public GameObject Gamespanel;
    public GameObject BlockLevelsPanels;

    [Header(" Text Objects")]
    public TMP_Text pack1Text;
    public TMP_Text pack2Text;
    public TMP_Text pack3Text;

    [Header(" Local Variables")]
    public int freeLevelCount = 50;
    public int paidLevelCount = 100;
    public int paidLevelPack3Count = 100;

    private void OnEnable()
    {
        pack1Text.text = "Pack1 (" + playerData.slideTheBlockFreeLevelsUnlocked + "/" + freeLevelCount+")";
        pack2Text.text = "Pack2 (" + playerData.slideTheBlockPaidLevelsUnlocked + "/" + paidLevelCount+")";
        pack3Text.text = "Pack3 (" + playerData.slideTheBlockPaidLevelsPack3Unlocked + "/" + paidLevelPack3Count + ")";
    }

    public void FreeLevelsButtonClicked()
    {
        audioData.PlayButtonClickSound();
        uiData.currentBlocksLevelsType = SlideTheBlocklevelsType.Free;
        BlockLevelsPanels.SetActive(true);
        gameObject.SetActive(false);
    }

    public void PaidLevelsButtonClicked()
    {
        audioData.PlayButtonClickSound();
        uiData.currentBlocksLevelsType = SlideTheBlocklevelsType.Paid;
        BlockLevelsPanels.SetActive(true);
        gameObject.SetActive(false);
    }


    public void PaidLevelsPack3ButtonClicked()
    {
        audioData.PlayButtonClickSound();
        uiData.currentBlocksLevelsType = SlideTheBlocklevelsType.Pack3;
        BlockLevelsPanels.SetActive(true);
        gameObject.SetActive(false);
    }
    public void BackBtnClicked()
    {
        audioData.PlayButtonClickSound();
        Gamespanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
