using UnityEngine;

public class PassthroughManager : MonoBehaviour
{

    [Header("Objects")]
    public GameObject room;
    public GameObject passThroughLayer;

    [Header("Cams")]
    public Camera leftEye;
    public Camera centerEye;
    public Camera rightEye;


  
    public void SwitchToVR()
    {
        leftEye.clearFlags = CameraClearFlags.Skybox;
        rightEye.clearFlags = CameraClearFlags.Skybox;
        centerEye.clearFlags = CameraClearFlags.Skybox;

        passThroughLayer.SetActive(false);
        room.SetActive(true);
        
    }

    public void SwitchToMR()
    {
        leftEye.clearFlags = CameraClearFlags.SolidColor;
        rightEye.clearFlags = CameraClearFlags.SolidColor;
        centerEye.clearFlags = CameraClearFlags.SolidColor;

        passThroughLayer.SetActive(true);
        room.SetActive(false);

    }
}
