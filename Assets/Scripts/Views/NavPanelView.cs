using com.VisionXR.Models;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavPanelView : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public AudioDataSO audioData;
    public UIDataSO uiData;

    [Header(" UI Objects")]
    public List<GameObject> panels;
    public List<Image> panelImages;


    private void OnEnable()
    {
        uiData.GameChangedEvent += GameChanged;
    }

    private void OnDisable()
    {
        uiData.GameChangedEvent -= GameChanged;
    }

    private void GameChanged()
    {
        //ResetPanels();
        //panels[2].SetActive(true);
        //panelImages[2].gameObject.SetActive(true);
    }

    private void Start()
    {
        ResetPanels();
        panels[0].SetActive(true);
        panelImages[0].gameObject.SetActive(true);
    }

    public void PanelButtonClicked(int id)
    {
        audioData.PlayButtonClickSound();
        ResetPanels();
        panels[id].SetActive(true);
        panelImages[id].gameObject.SetActive(true);
    }


    public void ResetPanels()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false); 
        }

        foreach (Image panelImage in panelImages)
        {
           panelImage.gameObject.SetActive(false);
        }
    }
}
