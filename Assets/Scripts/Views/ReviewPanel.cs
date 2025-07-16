using com.VisionXR.Models;
using UnityEngine;

public class ReviewPanel : MonoBehaviour
{
    [Header("Scriptable objects")]

    public AudioDataSO audioData;
    public string url;

    public void RatingBtnClicked()
    {
        audioData.PlayButtonClickSound();
        Application.OpenURL(url);
    }
}
