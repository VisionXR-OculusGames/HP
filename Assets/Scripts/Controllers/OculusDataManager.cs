using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Oculus.Platform;
using com.VisionXR.Models;
using UnityEngine.Networking;

namespace com.visionXR.Controllers
{
    public class OculusDataManager : MonoBehaviour
    {
        [Header("Scriptable objects")]
        public PlayerDataSO playerData;


        // local variables
        private ulong _userId = 0;
        private bool isEntitlementVerified = false;

        public void BeginLogin()
        {
            if (!UnityEngine.Application.isEditor)
            {
                StartCoroutine(LoginToOculus());
            }
            else
            {
                playerData.playerId = 1234;
                playerData.playerName = "Vikky";
                playerData.SignInToUnity("test-nonce", "81433");
            }
        }

        public IEnumerator LoginToOculus()
        {
            while (!isEntitlementVerified)
            {
                // Initialize the Oculus Platform
                Core.AsyncInitialize();
                // Do the entitlement check
                Entitlements.IsUserEntitledToApplication().OnComplete(EntitlementCallback);
                yield return new WaitForSeconds(10);
            }
        }

        private void EntitlementCallback(Message message)
        {
            if (message.IsError)
            {
                isEntitlementVerified = false;
                UnityEngine.Application.Quit();
            }
            else
            {
                Debug.Log("✅ Game Entitlement Verified.");
                isEntitlementVerified = true;
                StartCoroutine(StartOVRPlatform());
            }
        }

        private IEnumerator StartOVRPlatform()
        {
            yield return new WaitForSeconds(1);

            // user ID == 0 means we want to load logged-in user avatar from CDN
            while (!playerData.isLoggedIn)
            {
                // Get User ID
                LogInToOculus();
                yield return new WaitForSeconds(5);
            }
        }

        private async void LogInToOculus()
        {
            // Step 1: Get Nonce and User ID
            (string nonce, string userId) = await GetOculusNonceAndUserID();

            if (!string.IsNullOrEmpty(nonce) && !string.IsNullOrEmpty(userId))
            {
                // Step 2: Authenticate
                playerData.SignInToUnity(nonce, userId);
            }
            else
            {
                Debug.LogError("❌ Failed to retrieve Nonce or User ID.");
            }

            // Fetch user age category
            UserAgeCategory.Get().OnComplete((msg) =>
            {
                if (!msg.IsError)
                {
                    Debug.Log("👶 Age category: " + msg.Data.AgeCategory);
                }
                else
                {
                    Debug.LogError("❌ Error fetching age category.");
                }
            });
        }

        /// <summary>
        /// Fetches Oculus Nonce and User ID and waits for both before returning.
        /// </summary>
        private async Task<(string nonce, string userId)> GetOculusNonceAndUserID()
        {
            var nonceTask = new TaskCompletionSource<string>();
            var userIdTask = new TaskCompletionSource<string>();

            // Step 1: Get Nonce
            Users.GetUserProof().OnComplete(nonceMsg =>
            {
                if (nonceMsg.IsError)
                {
                    Debug.LogError("❌ Failed to get nonce from Oculus.");
                    nonceTask.SetResult(null);
                }
                else
                {
                    string nonce = nonceMsg.Data.Value;
                    Debug.Log($"✅ Received Nonce from Oculus: {nonce}");
                    nonceTask.SetResult(nonce);
                }
            });

            // Step 2: Get Oculus User ID
            Users.GetLoggedInUser().OnComplete(userMsg =>
            {
                if (userMsg.IsError)
                {
                    Debug.LogError("❌ Failed to retrieve Oculus User ID.");
                    userIdTask.SetResult(null);
                }
                else
                {
                    string userId = userMsg.Data.ID.ToString();
                    Debug.Log($"✅ Oculus User ID: {userId}");
                    userIdTask.SetResult(userId);

                    // Store in PlayerData
                    playerData.playerId = userMsg.Data.ID;
                    playerData.playerName = userMsg.Data.DisplayName;
                    StartCoroutine(FetchAvatarImageURL(userMsg.Data.ImageURL));

                }
            });

            // Step 3: Wait for Both Nonce and User ID
            string nonceResult = await nonceTask.Task;
            string userIdResult = await userIdTask.Task;

            return (nonceResult, userIdResult);
        }

        public IEnumerator FetchAvatarImageURL(string imageUrl)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(imageUrl))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Failed to fetch avatar image URL: " + www.error);
                    yield break;
                }

                string avatarImageUrl = www.url;
                // Use the avatar image URL as needed (e.g., load the image using UnityWebRequest)
                StartCoroutine(LoadAvatarImage(avatarImageUrl));
            }
        }

        private IEnumerator LoadAvatarImage(string url)
        {
            WWW www = new WWW(url);
            yield return www;

            if (www.error != null)
            {
                Debug.LogError("Failed to download avatar: " + www.error);
                yield break;
            }

            Texture2D texture = www.texture;
            Sprite s = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            playerData.SetProfileImage(s);

        }

     
    }
}
