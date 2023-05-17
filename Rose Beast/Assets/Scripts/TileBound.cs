using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System;

public class TileBound : MonoBehaviour
{ 
    private Tilemap tilemap;
    private Slider slider;
    public Color unitColor;
    
    public int age; //how many 10 second intervals have I been alive for?

    void Awake(){
         slider = transform.GetComponentInChildren<Slider>();
    }
    
    void Start()
    {
        unitColor = UnityEngine.Random.ColorHSV(0f, 0.5f, 1f, 1f, 1f, 1f, 0.2f, 0.4f);
          tilemap = FindObjectOfType<Tilemap>();
         transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(this.transform.position));
    }

    public void UpdateSlider(float fill, float max, Color color){
        if(slider != null){
            slider.maxValue = max;
            slider.value = fill;
            UpdateSliderColor(color);
        }
    }

    public void UpdateSliderColor(Color color){
        if(slider != null){
            Image fillProbably = slider.GetComponentInChildren<Image>();
            fillProbably.color = color;
        }
    }

    public void UpdateAge(){
        age++;
    }
    
}
