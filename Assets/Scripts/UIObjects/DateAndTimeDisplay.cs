using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using com.VisionXR.Models;

public class DateAndTimeDisplay : MonoBehaviour
{
 

    [Header("UI Objects")]
    public TMP_Text dateTimeText; 
    public Image dislayImage;




    private void UpdateImage(Sprite sprite)
    {
        dislayImage.sprite = sprite;    
    }

    private void Start()
    {
        UpdateDateTime(); // Initial update
        InvokeRepeating(nameof(UpdateDateTime), 0, 60); // Update every minute
    }

    private void UpdateDateTime()
    {
        // Get the current time and day
        DateTime now = DateTime.Now;
        string formattedTime = now.ToString("hh:mm tt"); // 12-hour format (AM/PM)
        string dayOfWeek = now.DayOfWeek.ToString(); // Get full day name (e.g., Sunday)

        // Set the text
        dateTimeText.text = $"{formattedTime} {dayOfWeek}";
    }
}
