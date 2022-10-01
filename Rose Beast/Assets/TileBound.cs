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
    
    public int age; //how many 10 second intervals have I been alive for?

    void Awake(){
         slider = transform.GetComponentInChildren<Slider>();
    }
    
    void Start()
    {
          tilemap = FindObjectOfType<Tilemap>();
         transform.position = tilemap.GetCellCenterWorld(tilemap.WorldToCell(this.transform.position));
    }

    public void UpdateSlider(int fill, int max, Color color){
        if(slider != null){
            slider.value = fill;
            slider.maxValue = max;
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
