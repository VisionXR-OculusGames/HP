using Oculus.Platform;
using Oculus.Platform.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.VisionXR.ModelClasses
{
    [CreateAssetMenu(fileName = "LeaderBoardSO", menuName = "ScriptableObjects/LeaderBoardSO", order = 1)]
    public class LeaderBoardSO : ScriptableObject
    {
        [Header("Leader board ids ")]
        public string matchStickId;
        public string brainvitaId;
        public string slideTheBlockId;
        public string tangramId;
        public string towersOfHenoiId;

        // Action
        public Action GetTop10EntriesEvent;
        public Action GetTop10EntriesArounduserEvent;
        public Action GetMyPointsEvent;
      

        public Action<List<string>, List<int>, List<int>> ShowLeaderBoardDataEvent;
        public Action<int,int> ShowMyDataEvent;


        // Methods

        public void WriteToSlideTheBlock(int points)
        {
            try
            {
                Leaderboards.WriteEntry(slideTheBlockId, points);

            }
            catch (Exception e)
            {
                Debug.Log(" Some error " + e.Message);
            }
        }

        public void WriteToMatchStick(int points)
        {
            try
            {
                Leaderboards.WriteEntry(matchStickId, points);

            }
            catch (Exception e)
            {
                Debug.Log(" Some error " + e.Message);
            }
        }

        public void WriteToBrainvita(int points)
        {
            try
            {
                Leaderboards.WriteEntry(brainvitaId, points);

            }
            catch (Exception e)
            {
                Debug.Log(" Some error " + e.Message);
            }
        }

        public void WriteToTangram(int points)
        {
            try
            {
                Leaderboards.WriteEntry(tangramId, points);
            }
            catch (Exception e)
            {
                Debug.Log(" Some error " + e.Message);
            }
        }

        public void WriteToTowersOfHenoi(int points)
        {
            try
            {
                Leaderboards.WriteEntry(towersOfHenoiId, points);
            }
            catch (Exception e)
            {
                Debug.Log(" Some error " + e.Message);
            }
        }


        public void GetTopTenEntries(string id)
        {
            Leaderboards.GetEntries(id, 10, LeaderboardFilterType.None, LeaderboardStartAt.Top).OnComplete(GetTop10Callback);
        }

        public void GetMyPoints(string id)
        {
            Leaderboards.GetEntries(id, 1, LeaderboardFilterType.None, LeaderboardStartAt.CenteredOnViewer).OnComplete(GetUserPoints);
        }
        void GetTop10Callback(Message<LeaderboardEntryList> msg)
        {
            List<string> names = new List<string>();
            List<int> ranks = new List<int>();
            List<int> points = new List<int>();
            if (!msg.IsError)
            {
                foreach (var entry in msg.Data)
                {
                    names.Add(entry.User.DisplayName);
                    ranks.Add(entry.Rank);
                    points.Add((int)entry.Score);
                    // Use entry.User.OculusID, entry.Score, etc.
                }
            }

            ShowLeaderBoardDataEvent?.Invoke(names, ranks, points);
           
        }

        void GetUserPoints(Message<LeaderboardEntryList> msg)
        {

            if (!msg.IsError)
            {
                foreach (var entry in msg.Data)
                {
                     ShowMyDataEvent?.Invoke((int)entry.Rank,(int)entry.Score);
                    
                }
            }
        }


    }
}
