using System.Collections;
using UnityEngine;

public class ControllerAutoDisabler : MonoBehaviour
{
    [Tooltip("Time after which controller tracking is disabled")]
    public float disableAfterSeconds = 1f;

    private void Start()
    {
        StartCoroutine(DisableControllerTracking());
    }

    private IEnumerator DisableControllerTracking()
    {
        yield return new WaitForSeconds(disableAfterSeconds);
        gameObject.SetActive(false);
        Debug.Log("🛑 Controller tracking disabled after delay.");
    }
}
