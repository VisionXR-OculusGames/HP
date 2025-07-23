using com.VisionXR.ModelClasses;
using com.VisionXR.Models;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardPanel : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public AudioDataSO audioData;
    public LeaderBoardSO leaderBoardData;

    [Header(" My Texts")]
    public TMP_Text myRank;
    public TMP_Text myPoints;

    [Header(" Texts")]
    public List<TMP_Text> ranksText;
    public List<TMP_Text> namesText;
    public List<TMP_Text> pointsText;
    

    [Header(" Images ")]
    public GameObject MatchStickImage;
    public GameObject SlideTheBlockImage;
    public GameObject BrainvitaImage;
    public GameObject TangramImage;
    public GameObject TowersOfHenoiImage;
    private void OnEnable()
    {
        leaderBoardData.ShowLeaderBoardDataEvent += DisplayData;
        leaderBoardData.ShowMyDataEvent += DisplayMyData;

        ResetImages();
        MatchStickImage.SetActive(true);
        leaderBoardData.GetTopTenEntries(leaderBoardData.matchStickId);
        leaderBoardData.GetMyPoints(leaderBoardData.matchStickId);
      
    }

    private void OnDisable()
    {
        leaderBoardData.ShowLeaderBoardDataEvent -= DisplayData;
        leaderBoardData.ShowMyDataEvent -= DisplayMyData;
    }

    private void DisplayMyData(int rank, int points)
    {
        myRank.text = "My Rank " + rank;
        myPoints.text = "My Points " + points;
    }

    private void DisplayData(List<string> Names, List<int> Ranks, List<int> Points)
    {
       
        for (int i = 0 ;i< Ranks.Count; i++) {

            ranksText[i].text = Ranks[i].ToString();
            namesText[i].text = Names[i];
            pointsText[i].text = Points[i].ToString();     
        }
    }

    private void ResetTexts()
    {
        for (int i = 0; i < ranksText.Count; i++)
        {

            ranksText[i].text = "";
            namesText[i].text = "";
            pointsText[i].text = "";
        }
    }
    public void MatchStickBtnClicked()
    {
        audioData.PlayButtonClickSound();
        ResetImages();
        MatchStickImage.SetActive(true);
        ResetTexts();
        leaderBoardData.GetTopTenEntries(leaderBoardData.matchStickId);
        leaderBoardData.GetMyPoints(leaderBoardData.matchStickId);
    }

    public void SlideTheBlockBtnClicked()
    {
        audioData.PlayButtonClickSound();
        ResetImages();
        SlideTheBlockImage.SetActive(true);
        ResetTexts();
        leaderBoardData.GetTopTenEntries(leaderBoardData.slideTheBlockId);
        leaderBoardData.GetMyPoints(leaderBoardData.slideTheBlockId);
    }

    public void TangramClicked()
    {
        audioData.PlayButtonClickSound();
        ResetImages();
        TangramImage.SetActive(true);
        ResetTexts();
        leaderBoardData.GetTopTenEntries(leaderBoardData.tangramId);
        leaderBoardData.GetMyPoints(leaderBoardData.tangramId);
    }

    public void TowersOfHenoiClicked()
    {
        audioData.PlayButtonClickSound();
        ResetImages();
        TowersOfHenoiImage.SetActive(true);
        ResetTexts();
        leaderBoardData.GetTopTenEntries(leaderBoardData.towersOfHenoiId);
        leaderBoardData.GetMyPoints(leaderBoardData.towersOfHenoiId);
    }

    public void BrainvitaClicked()
    {
        audioData.PlayButtonClickSound();
        ResetImages();
        BrainvitaImage.SetActive(true);
        ResetTexts();
        leaderBoardData.GetTopTenEntries(leaderBoardData.brainvitaId);
        leaderBoardData.GetMyPoints(leaderBoardData.brainvitaId);
    }

    void ResetImages()
    {
        BrainvitaImage.SetActive(false);
        SlideTheBlockImage.SetActive(false);
        MatchStickImage.SetActive(false);
        TangramImage.SetActive(false);
        TowersOfHenoiImage.SetActive(false);

    }
}
