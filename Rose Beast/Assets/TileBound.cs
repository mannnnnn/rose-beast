using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

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

    public void UpdateSlider(int fill, int max){
        if(slider != null){
            slider.value = fill;
            slider.maxValue = max;
        }
    }

    public void UpdateAge(){
        age++;
    }

    
}
