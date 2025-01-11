using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPopUp : MonoBehaviour
{
    public TMPro.TextMeshProUGUI TextPopped;

    public void SetText(float value, string hue, float fontSize = 34f)
    {
        TextPopped.fontSize = fontSize;
        switch (hue)
        {
            case "white":
                TextPopped.color = new Color(1, 1f, 1f, 1);
                break;
            case "green":
                TextPopped.color = new Color(0.388f, 0.709f, 0.063f, 1);
                break;
            case "orange":
                TextPopped.color = new Color(0.9f, 0.57f, 0.22f, 1);
                break;
            case "yellow":
                TextPopped.color = new Color(1, 0.85f, 0.4f, 1);
                //TextPopped.fontSize += 0.15f + damageText.fontSize * 0.05f;
                break;
            case "cyan":
                TextPopped.color = new Color(0.361f, 0.847f, 0.925f, 1);
                break;
            case "purple":
                TextPopped.color = new Color(0.5f, 0.1f, 0.9f, 1);
                break;
        }
        TextPopped.text = value.ToString("0");
    }
}
