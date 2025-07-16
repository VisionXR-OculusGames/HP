using com.VisionXR.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectButtonStates : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public AppSettings settings;

    [SerializeField] private Image BackgroundImage;
    [SerializeField] private Image HoverImage;

    private bool isHovering = false;


    void OnEnable()
    {
        Invoke("SetHover", 0.05f);
    }

    void SetHover()
    {
        HoverImage.color = settings.HoverIdle;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isHovering && !BackgroundImage.gameObject.activeInHierarchy)
        {         
            HoverImage.color = settings.HoverColor;
            isHovering = true;
            
        }
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isHovering && !BackgroundImage.gameObject.activeInHierarchy)
        {
            isHovering = false;
            HoverImage.color = settings.HoverIdle;
        }
       
    }

    public void OnPointerClick(PointerEventData eventData)
    {              
         if(isHovering)
        {
            isHovering = false;
            HoverImage.color = settings.HoverIdle;
        }
        BackgroundImage.enabled = true;
    }

    public void PlayHapticVibration()
    {
        OVRInput.SetControllerVibration(settings.vibrationAmplitude, settings.vibrationAmplitude, OVRInput.Controller.RTouch);
        Invoke("StopVibration", settings.vibrationDuration);
    }
    private void StopVibration()
    {
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }
}
