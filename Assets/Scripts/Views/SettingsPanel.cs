using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    public AudioSource bgMusic;
    public PassthroughManager passthroughManager;

    public void OnVolumeChanged(float val)
    {
        bgMusic.volume = val;
    }

    public void OnModeChanged(float val)
    {
        if(val == 0)
        {
            passthroughManager.SwitchToMR();
        }
        else
        {
            passthroughManager.SwitchToVR();
        }
    }
}
