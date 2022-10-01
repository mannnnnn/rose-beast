using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rose : Grower
{
   public List<Sprite> growingSprites = new List<Sprite>();
    public List<int> requiredGrowingAges = new List<int>();

    void Start()
    {
        tile = GetComponent<TileBound>();
        growingAge = requiredGrowingAges[0];
        tile.UpdateSlider(0, growingAge, Color.white);
    }

    public override void Grow(){
        FindObjectOfType<BoundsController>().ExpandBounds();
        GetComponentInChildren<SpriteRenderer>().sprite = growingSprites[growingSprites.IndexOf(GetComponentInChildren<SpriteRenderer>().sprite)+1];
   
        if(requiredGrowingAges.IndexOf(growingAge) < requiredGrowingAges.Count-1){
            growingAge = requiredGrowingAges[requiredGrowingAges.IndexOf(growingAge)];
        } else {
            growingAge = -1;
        }
    }

}
