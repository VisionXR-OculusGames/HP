using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using com.VisionXR.Models;
using Unity.Services.CloudSave;
using System;

public class AuthenticationManager : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public AppSettings appSettings;
    public PlayerDataSO playerData;

    [Header(" Cloud variables Do not change")]
    public string brainVitaKey = "BrainVitaData";
    public string matchStickKey = "MatchStickData";
    public string slideTheBlockKey = "SlideTheBlockData";
    public string tangramKey = "TangramData";

    private void OnEnable()
    {
        playerData.SignInToUnityEvent += SignIn;
        playerData.UnlockBrainVitaLevelsEvent += BrainvitaPurchaseSuccess;
        playerData.UnlockMatchStickLevelsEvent += MatchStickPurchaseSuccess;
        playerData.UnlockSlideTheBlockLevelsEvent += SlideTheBlockPurchaseSuccess;
        playerData.UnlockTangramLevelsEvent += TangramPurchaseSuccess;
    }

    private void OnDisable()
    {
        playerData.SignInToUnityEvent -= SignIn;
        playerData.UnlockBrainVitaLevelsEvent -= BrainvitaPurchaseSuccess;
        playerData.UnlockMatchStickLevelsEvent -= MatchStickPurchaseSuccess;
        playerData.UnlockSlideTheBlockLevelsEvent -= SlideTheBlockPurchaseSuccess;
        playerData.UnlockTangramLevelsEvent -= TangramPurchaseSuccess;
    }

    private void BrainvitaPurchaseSuccess()
    {
        SaveBrainvitaData();
    }

    private void MatchStickPurchaseSuccess()
    {
        SaveMatchStickData();
    }

    private void SlideTheBlockPurchaseSuccess()
    {
       
        SaveSlideTheBlockData();
    }

    private void TangramPurchaseSuccess()
    {
        SaveTangramData();
    }


    public async void SaveTangramData()
    {
        bool isSuccess = await SaveTangramCloudData();
        if (isSuccess)
        {

        }
        else
        {
            await Task.Delay(5000); // wait for 5 seconds before retrying
            SaveTangramData(); // retry
        }
    }
    public async void SaveBrainvitaData()
    {
        bool isSuccess = await SaveBrainvitaCloudData();
        if (isSuccess)
        {
            
        }
        else
        {
            await Task.Delay(5000); // wait for 5 seconds before retrying
            SaveBrainvitaData(); // retry
        }
    }

    public async void SaveSlideTheBlockData()
    {
        bool isSuccess = await SaveSlideTheBlockCloudData();
        if (isSuccess)
        {
            
        }
        else
        {
            await Task.Delay(5000); // wait for 5 seconds before retrying
            SaveSlideTheBlockData(); // retry
        }
    }

    public async void SaveMatchStickData()
    {
        bool isSuccess = await SaveMatchStickCloudData();
        if (isSuccess)
        {
           
        }
        else
        {
            await Task.Delay(5000); // wait for 5 seconds before retrying
            SaveMatchStickData(); // retry
        }
    }

    private void SignIn(string nonce, string oculusID)
    {
        SignInWithOculusID(nonce, oculusID);
    }


    /// <summary>
    /// Signs in the player using Oculus ID as a custom external token.
    /// </summary>
    public async void SignInWithOculusID(string nonce, string oculusID)
    {
        await InitializeUnityServices();

        if (AuthenticationService.Instance.IsSignedIn)
        {
            Debug.Log("Already signed in: " + AuthenticationService.Instance.PlayerId);
            playerData.LoggedIn();
            return;
        }

        try
        {
            if (Application.isEditor)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                Debug.Log("Editor Sign-in successful. Test Player ID: " + AuthenticationService.Instance.PlayerId);
            }
            else
            {

                // Step 1: Generate a secure nonce              
                await AuthenticationService.Instance.SignInWithOculusAsync(nonce, oculusID);
                playerData.LoggedIn();
                Debug.Log("Custom ID Sign-in successful.");
            }

            await LoadAllCloudData();
        }

        catch (AuthenticationException e)
        {
            Debug.LogError("Custom ID sign-in failed: " + e.Message);
        }
        catch (RequestFailedException e)
        {
            Debug.LogError("Sign-in request failed: " + e.Message);
        }
    }

    /// <summary>
    /// Initializes Unity Gaming Services.
    /// </summary>
    private async Task InitializeUnityServices()
    {
        if (!UnityServices.State.Equals(ServicesInitializationState.Initialized))
        {
            await UnityServices.InitializeAsync();
        }
    }

    /// <summary>
    /// Saves player data (e.g., old user status, premium access) to Unity Cloud Save.
    /// </summary>
    public async Task<bool> SaveBrainvitaCloudData()
    {

        BrainVitaData cloudData = new BrainVitaData();
        cloudData.isBrainvitaLevelsBought = true;
        try
        {
            string jsonData = JsonUtility.ToJson(cloudData);
            var data = new Dictionary<string, object> { { brainVitaKey, jsonData } };

            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            Debug.Log(" Data Saved");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save cloud data: {e.Message}");
            return false;
        }
    }

    public async Task<bool> SaveSlideTheBlockCloudData()
    {

        SlideTheBlockData cloudData = new SlideTheBlockData();
        cloudData.isSlideTheBlockLevelsBought = true;
        try
        {
            string jsonData = JsonUtility.ToJson(cloudData);
            var data = new Dictionary<string, object> { { slideTheBlockKey, jsonData } };

            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            Debug.Log(" Data Saved");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save cloud data: {e.Message}");
            return false;
        }
    }

    public async Task<bool> SaveMatchStickCloudData()
    {

        MatchStickData cloudData = new MatchStickData();
        cloudData.isMatchStickLevelsBought = true;
        try
        {
            string jsonData = JsonUtility.ToJson(cloudData);
            var data = new Dictionary<string, object> { { matchStickKey, jsonData } };

            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            Debug.Log(" Data Saved");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save cloud data: {e.Message}");
            return false;
        }
    }

    public async Task<bool> SaveTangramCloudData()
    {
        TangramData cloudData = new TangramData();
        cloudData.isTangramLevelsBought = true;
        try
        {
            string jsonData = JsonUtility.ToJson(cloudData);
            var data = new Dictionary<string, object> { { tangramKey, jsonData } };
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            Debug.Log(" Data Saved");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save cloud data: {e.Message}");
            return false;
        }
    }

    public async Task LoadAllCloudData()
    {
        Task<SlideTheBlockData> slideTask = LoadSlideTheBlockData();
        Task<MatchStickData> matchTask = LoadMatchStickData();
        Task<BrainVitaData> brainTask = LoadBrainVitaData();
        Task<TangramData> tangramTask = LoadTangramData();

        await Task.WhenAll(slideTask, matchTask, brainTask, tangramTask);

        // Assign to playerData or use results
        playerData.isSlideTheBlockLevelsUnlocked = slideTask.Result?.isSlideTheBlockLevelsBought ?? false;
        playerData.isMatchSticksLevelsUnlocked = matchTask.Result?.isMatchStickLevelsBought ?? false;
        playerData.isBrainVitaLevelsUnlocked = brainTask.Result?.isBrainvitaLevelsBought ?? false;
        playerData.isTangramLevelsUnlocked = tangramTask.Result?.isTangramLevelsBought ?? false;

        Debug.Log("? All cloud data loaded.");
        //playerData.CloudDataLoaded(); // Raise an event or flag to notify completion
    }

    public async Task<SlideTheBlockData> LoadSlideTheBlockData()
    {
        try
        {
            var data = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { slideTheBlockKey });
            if (data.TryGetValue(slideTheBlockKey, out var item))
            {
                return JsonUtility.FromJson<SlideTheBlockData>(item.Value.GetAs<string>());
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load SlideTheBlock data: {e.Message}");
        }
        return null;
    }

    public async Task<MatchStickData> LoadMatchStickData()
    {
        try
        {
            var data = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { matchStickKey });
            if (data.TryGetValue(matchStickKey, out var item))
            {
                return JsonUtility.FromJson<MatchStickData>(item.Value.GetAs<string>());
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load MatchStick data: {e.Message}");
        }
        return null;
    }

    public async Task<BrainVitaData> LoadBrainVitaData()
    {
        try
        {
            var data = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { brainVitaKey });
            if (data.TryGetValue(brainVitaKey, out var item))
            {
                return JsonUtility.FromJson<BrainVitaData>(item.Value.GetAs<string>());
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load BrainVita data: {e.Message}");
        }
        return null;
    }

    public async Task<TangramData> LoadTangramData()
    {
        try
        {
            var data = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { tangramKey });
            if (data.TryGetValue(tangramKey, out var item))
            {
                return JsonUtility.FromJson<TangramData>(item.Value.GetAs<string>());
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load Tangram data: {e.Message}");
        }
        return null;
    }


}

[Serializable]
public class SlideTheBlockData
{
    public bool isSlideTheBlockLevelsBought;

}

[Serializable]
public class MatchStickData
{
    public bool isMatchStickLevelsBought;

}

[Serializable]
public class BrainVitaData
{
    public bool isBrainvitaLevelsBought;

}

[Serializable]
public class TangramData
{
    public bool isTangramLevelsBought;
}


