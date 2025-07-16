using com.VisionXR.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewObjectButtonStates : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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
        HoverImage.color = settings.OtherIdleColor;
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
            HoverImage.color = settings.OtherIdleColor;
        }
       
    }

    public void OnPointerClick(PointerEventData eventData)
    {              
         if(isHovering)
        {
            isHovering = false;
            HoverImage.color = settings.OtherIdleColor;
        }
        BackgroundImage.enabled = true;
    }

   
}
