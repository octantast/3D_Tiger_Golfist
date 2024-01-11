using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fading : MonoBehaviour
{
    private Image thisImage;
    private TMP_Text thisText;
    public Color32 targetColor;
    public float fadeduration;
    void Start()
    {
        thisImage = transform.GetComponent<Image>();
        thisText = transform.GetComponent<TMP_Text>();
    }
    private void Update()
    {
        if (thisImage != null)
        {
            thisImage.color = Color.Lerp(thisImage.color, targetColor, fadeduration * Time.deltaTime);
        }
        if(thisText != null)
        {
            thisText.color = Color.Lerp(thisText.color, targetColor, fadeduration * Time.deltaTime);
        }
    }
}
