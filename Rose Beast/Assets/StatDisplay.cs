using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatDisplay : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI text;
    public void Initialize(Sprite icon, string text)
    {
        this.icon.sprite = icon;
        this.text.text = text;
    }
    public void ShowTooltip()
    {
        UnityEngine.Debug.Log("Stubbed");
    }
}
