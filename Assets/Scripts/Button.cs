using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
   private Image thisImage;
    private float neededSize;
    private float startSize;
    private Color32 neededColor;
    private Color32 startColor;

    void Start()
    {
        thisImage = transform.GetComponent<Image>();
        startSize = transform.localScale.x;
        startColor = thisImage.color;
        neededSize = startSize;
    }
    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(startSize, startSize, startSize), 10 * Time.deltaTime);
        //thisImage.color = Color.Lerp(thisImage.color, startColor, 10 * Time.deltaTime);
    }
    public void OnEnter()
    {
        transform.localScale = new Vector3(startSize + 0.1f, startSize + 0.1f, startSize + 0.1f);
        //thisImage.color = new Color32(150, 150, 150, 255);
    }
    public void OnExit()
    {
        transform.localScale = new Vector3(startSize, startSize, startSize);
        //neededColor = startColor;
    }

}
