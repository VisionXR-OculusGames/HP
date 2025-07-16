using System;
using UnityEngine;

namespace com.VisionXR.Models
{
    [CreateAssetMenu(fileName = "PlayerDataSO", menuName = "ScriptableObjects/PlayerDataSO", order = 1)]
    public class PlayerDataSO : ScriptableObject
    {
        // Variables
        public ulong playerId;
        public string playerName;
        public Sprite playerImage;
        public bool isLoggedIn = false;
        public string key;

        [Header(" Paid Levels Data")]
        public bool isBrainVitaLevelsUnlocked = false;
        public bool isSlideTheBlockLevelsUnlocked = false;
        public bool isMatchSticksLevelsUnlocked = false;


        [Header("  Levels Lock Data")]
        public int brainvitaFreeLevelsUnlocked = 1;
        public int brainvitaPaidLevelsUnlocked = 1;

        public int henoiFreeLevelsUnlocked = 1;
        public int henoiPaidLevelsUnlocked = 1;

        public int slideTheBlockFreeLevelsUnlocked = 1;
        public int slideTheBlockPaidLevelsUnlocked = 1;
        public int slideTheBlockPaidLevelsPack3Unlocked = 1;

        public int matchStickFreeLevelsUnlocked = 1;
        public int matchStickPaidLevelsUnlocked = 1;
        public int matchStickPaidLevelspack3Unlocked = 1;


        //Actions

        public Action<Sprite> PlayerImageChangedEvent;
        public Action playerLoggedInEvent;
        public Action<string, string> SignInToUnityEvent;


        public Action BuyBrainVitaLevelsEvent;
        public Action BuySlideTheBlockLevelsEvent;
        public Action BuyMatchStickLevelsEvent;


        public Action UnlockBrainVitaLevelsEvent;
        public Action UnlockSlideTheBlockLevelsEvent;
        public Action UnlockMatchStickLevelsEvent;



        //Method

        private void OnEnable()
        {
            isBrainVitaLevelsUnlocked = false;
            isSlideTheBlockLevelsUnlocked = false;
            isMatchSticksLevelsUnlocked = false;

            LoadLevelsData();
        }

        public void SignInToUnity(string nonce, string oculusId)
        {
            SignInToUnityEvent?.Invoke(nonce, oculusId);
        }

        public void LoggedIn()
        {
            isLoggedIn = true;
        }

        public void SetProfileImage(Sprite s)
        {
            playerImage = s;
            PlayerImageChangedEvent?.Invoke(s);
        }

        public void BuyBrainVitaLevels()
        {
            BuyBrainVitaLevelsEvent?.Invoke();
        }

        public void BuySlideTheBlockLevels()
        {
            BuySlideTheBlockLevelsEvent?.Invoke();
        }

        public void BuyMatchStickLevels()
        {
            BuyMatchStickLevelsEvent?.Invoke();
        }


        public void BrainVitaPurchaseSuccess()
        {
            isBrainVitaLevelsUnlocked = true;
            UnlockBrainVitaLevelsEvent?.Invoke();
        }

        public void SlideTheBlockPurchaseSuccess()
        {
            isSlideTheBlockLevelsUnlocked = true;
            UnlockSlideTheBlockLevelsEvent?.Invoke();
        }

        public void MatchStickPurchaseSuccess()
        {
            isMatchSticksLevelsUnlocked = true;
            UnlockMatchStickLevelsEvent?.Invoke();
        }

        public void SaveLevelsData()
        {
            LevelsLockedData data = new LevelsLockedData();

            data.brainvitaFreeLevelsUnlocked = brainvitaFreeLevelsUnlocked;
            data.brainvitaPaidLevelsUnlocked = brainvitaPaidLevelsUnlocked;

            data.henoiFreeLevelsUnlocked = henoiFreeLevelsUnlocked;
            data.henoiPaidLevelsUnlocked = henoiPaidLevelsUnlocked;

            data.slideTheBlockFreeLevelsUnlocked = slideTheBlockFreeLevelsUnlocked;
            data.slideTheBlockPaidLevelsUnlocked = slideTheBlockPaidLevelsUnlocked;

            data.matchStickFreeLevelsUnlocked = matchStickFreeLevelsUnlocked;
            data.matchStickPaidLevelsUnlocked = matchStickPaidLevelsUnlocked;

            data.slideTheBlockPaidLevelsPack3Unlocked = slideTheBlockPaidLevelsPack3Unlocked;
            data.matchStickPaidLevelsPack3Unlocked = matchStickPaidLevelspack3Unlocked;

            PlayerPrefs.SetString(key, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
        }

        public void LoadLevelsData()
        {
            if (PlayerPrefs.HasKey(key))
            {
                var data = JsonUtility.FromJson<LevelsLockedData>(PlayerPrefs.GetString(key));

                brainvitaFreeLevelsUnlocked = data.brainvitaFreeLevelsUnlocked;
                brainvitaPaidLevelsUnlocked = data.brainvitaPaidLevelsUnlocked;

                henoiFreeLevelsUnlocked = data.henoiFreeLevelsUnlocked;
                henoiPaidLevelsUnlocked = data.henoiPaidLevelsUnlocked;

                slideTheBlockFreeLevelsUnlocked = data.slideTheBlockFreeLevelsUnlocked;
                slideTheBlockPaidLevelsUnlocked = data.slideTheBlockPaidLevelsUnlocked;

                matchStickFreeLevelsUnlocked = data.matchStickFreeLevelsUnlocked;
                matchStickPaidLevelsUnlocked = data.matchStickPaidLevelsUnlocked;

                // Handle default for new field in case it's still 0
                slideTheBlockPaidLevelsPack3Unlocked =
                    data.slideTheBlockPaidLevelsPack3Unlocked > 0 ?
                    data.slideTheBlockPaidLevelsPack3Unlocked : 1;

                // Handle default for new field in case it's still 0
                matchStickPaidLevelspack3Unlocked =
                    data.matchStickPaidLevelsPack3Unlocked > 0 ?
                    data.matchStickPaidLevelsPack3Unlocked : 1;    
            }
        }
    }


    [Serializable]

    public class LevelsLockedData
    {
        public int brainvitaFreeLevelsUnlocked = 1;
        public int brainvitaPaidLevelsUnlocked = 1;

        public int henoiFreeLevelsUnlocked = 1;
        public int henoiPaidLevelsUnlocked = 1;

        public int slideTheBlockFreeLevelsUnlocked = 1;
        public int slideTheBlockPaidLevelsUnlocked = 1;
        

        public int matchStickFreeLevelsUnlocked = 1;
        public int matchStickPaidLevelsUnlocked = 1;

        // New Variables
        public int slideTheBlockPaidLevelsPack3Unlocked = 1;
        public int matchStickPaidLevelsPack3Unlocked = 1;
    }

 
}
