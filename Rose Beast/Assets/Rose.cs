using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rose : MonoBehaviour
{
    private TileBound tile;
    public List<Sprite> growingSprites = new List<Sprite>();
    public List<int> requiredGrowingAges = new List<int>();
    private int nextRequiredGrowingAge;

    void Start()
    {
        tile = GetComponent<TileBound>();
        nextRequiredGrowingAge = requiredGrowingAges[0];
        tile.UpdateSlider(0, nextRequiredGrowingAge);
    }

    void FixedUpdate(){
        if(nextRequiredGrowingAge >= 0){
             tile.UpdateSlider(tile.age, nextRequiredGrowingAge);
            if(tile.age >= nextRequiredGrowingAge){
                Grow();
                if(requiredGrowingAges.IndexOf(nextRequiredGrowingAge) < requiredGrowingAges.Count-1){
                    nextRequiredGrowingAge = requiredGrowingAges[requiredGrowingAges.IndexOf(nextRequiredGrowingAge)];
                } else {
                    nextRequiredGrowingAge = -1;
                }
            
            } 
        } else {
            tile.UpdateSlider(0,1);
        }

       
    }

    void Grow(){
        FindObjectOfType<BoundsController>().ExpandBounds();
        GetComponentInChildren<SpriteRenderer>().sprite = growingSprites[growingSprites.IndexOf(GetComponentInChildren<SpriteRenderer>().sprite)+1];
    }

    

}
