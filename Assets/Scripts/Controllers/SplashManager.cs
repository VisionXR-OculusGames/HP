using System.Collections;
using UnityEngine;

public class SplashManager : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Main UI root that contains all game UI (e.g. Canvas)")]
    public GameObject uiRoot;

    [Tooltip("Reference to OculusDataManager")]
    public com.visionXR.Controllers.OculusDataManager oculusManager;

    [Tooltip("Delay before starting login (to ensure UI shows up first)")]
    public float delayBeforeLogin = 2f;

    //[Header("Optional")]
    //public GameObject loadingIndicator; // a "Loading..." text or spinner

    [Header("Audio")]
    public AudioSource audioSource;

    private void Start()
    {

        PlayBackgroundMusic();
        if (uiRoot != null)
            uiRoot.SetActive(true); // Show UI immediately


        //if (loadingIndicator != null)
        //    loadingIndicator.SetActive(true);

        StartCoroutine(DelayedAuthentication());
    }

    private void PlayBackgroundMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private IEnumerator DelayedAuthentication()
    {
        yield return new WaitForSeconds(delayBeforeLogin);

        if (oculusManager != null)
        {
            oculusManager.BeginLogin(); // Custom method from modified OculusDataManager
        }
        else
        {
            Debug.LogError("❌ OculusManager not assigned in SplashManager.");
        }
    }
}
